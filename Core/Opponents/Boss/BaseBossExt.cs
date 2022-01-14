using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public abstract class BaseBossExt : Part1BossOpponent
	{
		public const string PrefabPathMasks = "Prefabs/Opponents/Leshy/Masks";
		public const string PrefabPathRoyalBossSkull = "Prefabs/Opponents/Grimora/RoyalBossSkull";

		public const Opponent.Type KayceeOpponent = (Type)1001;
		public const Opponent.Type DoggyOpponent = (Type)1002;
		public const Opponent.Type RoyalOpponent = (Type)1003;
		public const Opponent.Type GrimoraOpponent = (Type)1004;

		public static readonly Dictionary<string, Type> BossTypesByString = new()
		{
			{ "DoggyBoss", DoggyOpponent },
			{ "GrimoraBoss", GrimoraOpponent },
			{ "KayceeBoss", KayceeOpponent },
			{ "RoyalBoss", RoyalOpponent }
		};

		public static readonly Dictionary<Type, string> BossMasksByType = new()
		{
			{ DoggyOpponent, $"{PrefabPathMasks}/MaskProspector" },
			{ KayceeOpponent, $"{PrefabPathMasks}/MaskAngler" },
			{ RoyalOpponent, PrefabPathRoyalBossSkull }
		};

		public GameObject rightWrist = GameObject.Find("Grimora_RightWrist");

		public abstract StoryEvent EventForDefeat { get; }

		public abstract Opponent.Type Opponent { get; }

		public GameObject Mask { get; set; }

		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			yield return ReplaceBlueprintCustom(BuildInitialBlueprint());

			AudioController.Instance.FadeOutLoop(0.75f);
			RunState.CurrentMapRegion.FadeOutAmbientAudio();

			yield return base.IntroSequence(encounter);

			yield return new WaitForSeconds(0.15f);
			GrimoraAnimationController.Instance.ShowBossSkull();
			GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");

			if (this is not RoyalBossExt)
			{
				UnityEngine.Object.Destroy(GameObject.Find("RoyalBossSkull"));
			}

			if (BossMasksByType.TryGetValue(this.OpponentType, out string prefab))
			{
				Mask = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(prefab),
					GameObject.Find("Grimora_RightWrist").transform,
					true
				);
			}

			Mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
			Mask.transform.localRotation = Quaternion.Euler(0, 0, 260);
		}

		public virtual IEnumerator ReplaceBlueprintCustom(EncounterBlueprintData blueprintData)
		{
			base.Blueprint = blueprintData;
			List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
			this.ReplaceAndAppendTurnPlan(plan);
			yield return QueueNewCards();
		}

		public abstract EncounterBlueprintData BuildInitialBlueprint();
	}
}