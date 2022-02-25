using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public abstract class BaseBossExt : Part1BossOpponent
{
	public static readonly Dictionary<string, Tuple<Opponent.Type, System.Type, GameObject, EncounterBlueprintData>>
		OpponentTupleBySpecialId = new()
		{
			{
				"KayceeBoss", new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					KayceeOpponent,
					typeof(GrimoraModKayceeBossSequencer),
					AssetConstants.BossPieceKaycee,
					BlueprintUtils.BuildKayceeBossInitialBlueprint()
				)
			},
			{
				"SawyerBoss", new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					SawyerOpponent,
					typeof(GrimoraModSawyerBossSequencer),
					AssetConstants.BossPieceSawyer,
					BlueprintUtils.BuildSawyerBossInitialBlueprint()
				)
			},
			{
				"RoyalBoss", new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					RoyalOpponent,
					typeof(GrimoraModRoyalBossSequencer),
					AssetConstants.BossPieceRoyal.gameObject,
					BlueprintUtils.BuildRoyalBossInitialBlueprint()
				)
			},
			{
				"GrimoraBoss", new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					GrimoraOpponent,
					typeof(GrimoraModGrimoraBossSequencer),
					AssetConstants.BossPieceGrimora,
					BlueprintUtils.BuildGrimoraBossInitialBlueprint()
				)
			},
			{
				"GrimoraModBattleSequencer", new Tuple<Type, System.Type, GameObject, EncounterBlueprintData>(
					0,
					typeof(GrimoraModBattleSequencer),
					null,
					null
				)
			}
		};

	public GameObject RoyalBossSkull => GrimoraRightWrist.transform.GetChild(6).gameObject;

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
		{ RoyalOpponent, null },
	};

	private static readonly int ShowSkull = Animator.StringToHash("show_skull");
	private static readonly int HideSkull = Animator.StringToHash("hide_skull");


	public abstract StoryEvent EventForDefeat { get; }

	public abstract Type Opponent { get; }

	public abstract string SpecialEncounterId { get; }

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		yield return base.IntroSequence(encounter);

		// Royal boss has a specific sequence to follow so that it flows easier
		if (BossMasksByType.TryGetValue(Opponent, out GameObject skull))
		{
			Log.LogDebug($"[{GetType()}] Creating skull");
			if (this is RoyalBossOpponentExt)
			{
				GrimoraRightHandBossSkull = RoyalBossSkull;
				RoyalBossSkull.SetActive(true);
			}
			else
			{
				GrimoraRightHandBossSkull = Instantiate(skull, GrimoraRightWrist.transform);
				RoyalBossSkull.SetActive(false);
			}

			var bossSkullTransform = GrimoraRightHandBossSkull.transform;

			bossSkullTransform.localPosition = new Vector3(-0.0044f, 0.18f, -0.042f);
			bossSkullTransform.localRotation = Quaternion.Euler(85.85f, 227.76f, 262.77f);
			bossSkullTransform.localScale = new Vector3(0.14f, 0.14f, 0.14f);

			yield return ShowBossSkull();
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

			yield return HideBossSkull();

			DestroyScenery();

			SetSceneEffectsShown(false);

			AudioController.Instance.StopAllLoops();

			yield return new WaitForSeconds(0.75f);

			CleanUpBossBehaviours();

			ViewManager.Instance.SwitchToView(View.Default, false, true);

			TableVisualEffectsManager.Instance.ResetTableColors();
			yield return new WaitForSeconds(0.25f);

			Log.LogDebug($"Setting post battle special node to a rare code node data");
			TurnManager.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
		}
		else
		{
			yield return base.OutroSequence(false);
		}
	}

	public IEnumerator ShowBossSkull()
	{
		yield return new WaitForSeconds(0.1f);
		GrimoraAnimationController.Instance.ShowBossSkull();
		GrimoraAnimationController.Instance.headAnim.ResetTrigger(HideSkull);
		GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
		yield return new WaitForSeconds(0.05f);

		ViewManager.Instance.SwitchToView(View.BossCloseup, false, true);

		yield return new WaitForSeconds(1f);
	}

	public IEnumerator HideBossSkull()
	{
		if (GrimoraRightHandBossSkull is not null)
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
