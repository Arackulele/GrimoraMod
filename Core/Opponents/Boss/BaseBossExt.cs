using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{
	public abstract class BaseBossExt : Part1BossOpponent
	{
		public const string PrefabPathMasks = "Prefabs/Opponents/Leshy/Masks";
		public const string PrefabPathRoyalBossSkull = "Prefabs/Opponents/Grimora/RoyalBossSkull";

		private protected GameObject RoyalBossSkull => RightWrist.transform.GetChild(6).gameObject;
		public GameObject RightWrist { get; } = GameObject.Find("Grimora_RightWrist");

		public const Type KayceeOpponent = (Type)1001;
		public const Type DoggyOpponent = (Type)1002;
		public const Type RoyalOpponent = (Type)1003;
		public const Type GrimoraOpponent = (Type)1004;

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


		public abstract StoryEvent EventForDefeat { get; }

		public abstract Type Opponent { get; }

		protected internal GameObject Mask { get; set; }

		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			Log.LogDebug($"[{GetType()}] Calling ReplaceBlueprintCustom");
			yield return ReplaceBlueprintCustom(BuildInitialBlueprint());

			AudioController.Instance.FadeOutLoop(0.75f);
			RunState.CurrentMapRegion.FadeOutAmbientAudio();

			Log.LogDebug($"[{GetType()}] Calling IntroSequence");
			yield return base.IntroSequence(encounter);
			yield return new WaitForSeconds(1f);

			// Royal boss has a specific sequence to follow so that it flows easier
			if (this is not RoyalBossExt)
			{
				Log.LogDebug($"[{GetType()}] Setting RoyalBossSkull inactive");
				RoyalBossSkull.SetActive(false);
				if (BossMasksByType.TryGetValue(OpponentType, out string prefabPath))
				{
					Log.LogDebug($"[{GetType()}] Calling ShowBossSkull");
					GrimoraAnimationController.Instance.ShowBossSkull();

					Log.LogDebug($"[{GetType()}] Setting Head Trigger");
					GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");

					// Object.Destroy(RoyalBossSkull);

					Log.LogDebug($"[{GetType()}] Creating mask [{prefabPath}]");
					Mask = (GameObject)Instantiate(Resources.Load(prefabPath),
						RightWrist.transform,
						true
					);
				}
			}
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
}