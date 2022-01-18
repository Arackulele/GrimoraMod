using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public abstract class BaseBossExt : Part1BossOpponent
{
	public const string PrefabPathMasks = "Prefabs/Opponents/Leshy/Masks";
	public const string PrefabPathRoyalBossSkull = "Prefabs/Opponents/Grimora/RoyalBossSkull";

	private protected GameObject RoyalBossSkull => RightWrist.transform.GetChild(6).gameObject;
	public GameObject RightWrist { get; } = GameObject.Find("Grimora_RightWrist");

	public const Type KayceeOpponent = (Type)1001;
	public const Type SawyerOpponent = (Type)1002;
	public const Type RoyalOpponent = (Type)1003;
	public const Type GrimoraOpponent = (Type)1004;

	public static readonly Dictionary<string, Type> BossTypesByString = new()
	{
		{ SawyerBossOpponent.SpecialId, SawyerOpponent },
		{ GrimoraBossOpponentExt.SpecialId, GrimoraOpponent },
		{ KayceeBossOpponent.SpecialId, KayceeOpponent },
		{ RoyalBossOpponentExt.SpecialId, RoyalOpponent }
	};

	public static readonly Dictionary<Type, string> BossMasksByType = new()
	{
		{ SawyerOpponent, $"{PrefabPathMasks}/MaskTrader" },
		{ KayceeOpponent, $"{PrefabPathMasks}/MaskWoodcarver" },
		{ RoyalOpponent, PrefabPathRoyalBossSkull }
	};


	public abstract StoryEvent EventForDefeat { get; }

	public abstract Type Opponent { get; }

	protected internal GameObject Mask { get; set; }

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		// Log.LogDebug($"[{GetType()}] Calling IntroSequence");
		yield return base.IntroSequence(encounter);

		// Log.LogDebug($"[{GetType()}] Calling ReplaceBlueprintCustom");
		yield return ReplaceBlueprintCustom(BuildInitialBlueprint());

		// Royal boss has a specific sequence to follow so that it flows easier
		if (this is not RoyalBossOpponentExt && BossMasksByType.TryGetValue(OpponentType, out string prefabPath))
		{
			yield return ShowBossSkull();

			// Log.LogDebug($"[{GetType()}] Creating mask [{prefabPath}]");
			Mask = (GameObject)Instantiate(
				Resources.Load(prefabPath),
				RightWrist.transform
			);

			Mask.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			Mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
			Mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

			// UnityEngine.Object.Destroy(RoyalBossSkull);
			RoyalBossSkull.SetActive(false);
			yield return new WaitForSeconds(1f);

			AudioController.Instance.FadeOutLoop(0.75f);
			RunState.CurrentMapRegion.FadeOutAmbientAudio();
		}
	}

	public IEnumerator ShowBossSkull()
	{
		// Log.LogDebug($"[{GetType()}] Calling ShowBossSkull");
		GrimoraAnimationController.Instance.ShowBossSkull();

		// Log.LogDebug($"[{GetType()}] Setting Head Trigger");
		GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");

		yield return new WaitForSeconds(0.25f);

		ViewManager.Instance.SwitchToView(View.BossCloseup, immediate: false, lockAfter: true);
	}

	public virtual IEnumerator ReplaceBlueprintCustom(EncounterBlueprintData blueprintData)
	{
		Blueprint = blueprintData;
		List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(Blueprint, 0, false);
		ReplaceAndAppendTurnPlan(plan);
		yield return QueueNewCards();
	}

	public abstract EncounterBlueprintData BuildInitialBlueprint();
}