using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public abstract class BaseBossExt : Part1BossOpponent
{
	public static readonly Dictionary<string, Tuple<Type, System.Type, GameObject, EncounterBlueprintData>>
		OpponentTupleBySpecialId = new()
		{
			{
				GrimoraModKayceeBossSequencer.FullSequencer.Id,
				new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					KayceeOpponent,
					GrimoraModKayceeBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceKaycee,
					BlueprintUtils.BuildKayceeBossInitialBlueprint()
				)
			},
			{
				GrimoraModSawyerBossSequencer.FullSequencer.Id,
				new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					SawyerOpponent,
					GrimoraModSawyerBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceSawyer,
					BlueprintUtils.BuildSawyerBossInitialBlueprint()
				)
			},
			{
				GrimoraModRoyalBossSequencer.FullSequencer.Id, new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					RoyalOpponent,
					GrimoraModRoyalBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceRoyal.gameObject,
					BlueprintUtils.BuildRoyalBossInitialBlueprint()
				)
			},
			{
				GrimoraModGrimoraBossSequencer.FullSequencer.Id,
				new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					GrimoraOpponent,
					GrimoraModGrimoraBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceGrimora,
					BlueprintUtils.BuildGrimoraBossInitialBlueprint()
				)
			}
		};

	public GameObject GrimoraRightHandBossSkull
	{
		get => GrimoraAnimationController.Instance.bossSkull;
		set => GrimoraAnimationController.Instance.bossSkull = value;
	}

	public GameObject GrimoraRightWrist => GameObject.Find("Grimora_RightWrist");

	public const Type KayceeOpponent = (Type)1001;
	public const Type SawyerOpponent = (Type)1002;
	public const Type RoyalOpponent = (Type)1003;
	public const Type GrimoraOpponent = (Type)1004;

	public static readonly Dictionary<Type, GameObject> BossMasksByType = new()
	{
		{ KayceeOpponent, AssetConstants.BossSkullKaycee },
		{ SawyerOpponent, AssetConstants.BossSkullSawyer },
		{ RoyalOpponent, AssetConstants.BossSkullRoyal },
	};

	private static readonly int ShowSkull = Animator.StringToHash("show_skull");
	private static readonly int HideSkull = Animator.StringToHash("hide_skull");


	public abstract StoryEvent EventForDefeat { get; }

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		yield return base.IntroSequence(encounter);

		// Royal boss has a specific sequence to follow so that it flows easier
		if (BossMasksByType.TryGetValue(OpponentType, out GameObject skull))
		{
			Log.LogDebug($"[{GetType()}] Creating skull");

			GrimoraRightHandBossSkull = Instantiate(skull, GrimoraRightWrist.transform);

			var bossSkullTransform = GrimoraRightHandBossSkull.transform;

			bossSkullTransform.localPosition = new Vector3(-0.0044f, 0.18f, -0.042f);
			bossSkullTransform.localRotation = Quaternion.Euler(85.85f, 227.76f, 262.77f);
			bossSkullTransform.localScale = new Vector3(0.14f, 0.14f, 0.14f);

			yield return ShowBossSkullFromHand();
		}
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			ConfigHelper.Instance.SetBossDefeatedInConfig(this);
			if (ConfigHelper.Instance.BossesDefeated >= 4)
			{
				ConfigHelper.Instance.BossesDefeated = 0;
			}

			AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX);

			yield return HideRightHandBossSkull();

			DestroyScenery();

			SetSceneEffectsShown(false);

			AudioController.Instance.StopAllLoops();

			yield return new WaitForSeconds(0.75f);

			CleanUpBossBehaviours();

			ViewManager.Instance.SwitchToView(View.Default, false, true);

			TableVisualEffectsManager.Instance.ResetTableColors();
			yield return new WaitForSeconds(0.25f);

			TurnManager.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
		}
		else
		{
			yield return base.OutroSequence(false);
		}
	}

	public IEnumerator ShowBossSkullFromHand()
	{
		yield return new WaitForSeconds(0.1f);
		GrimoraAnimationController.Instance.ShowBossSkull();
		GrimoraAnimationController.Instance.headAnim.ResetTrigger(HideSkull);
		GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
		yield return new WaitForSeconds(0.05f);

		ViewManager.Instance.SwitchToView(View.BossCloseup, false, true);
	}

	public IEnumerator HideRightHandBossSkull()
	{
		if (GrimoraRightHandBossSkull.IsNotNull())
		{
			GrimoraAnimationController.Instance.GlitchOutBossSkull();
			GrimoraAnimationController.Instance.headAnim.ResetTrigger(ShowSkull);
			GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");
			GrimoraRightHandBossSkull = null;
		}

		yield break;
	}

	public abstract void PlayTheme();

	public virtual IEnumerator ReplaceBlueprintCustom(EncounterBlueprintData blueprintData)
	{
		Blueprint = blueprintData;
		List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(Blueprint, 0, false);
		ReplaceAndAppendTurnPlan(plan);
		yield return QueueNewCards();
	}
}
