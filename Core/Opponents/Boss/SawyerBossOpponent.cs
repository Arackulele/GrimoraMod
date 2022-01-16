using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod
{
	public class SawyerBossOpponent : BaseBossExt
	{
		public const string SpecialId = "SawyerBoss";

		public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;

		public override Type Opponent => SawyerOpponent;

		public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			AudioController.Instance.SetLoopAndPlay("boss_prospector_base");
			AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1);
			base.SpawnScenery("ForestTableEffects");
			yield return new WaitForSeconds(0.5f);

			AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f);
			yield return new WaitForSeconds(0.25f);

			ViewManager.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(1.25f);

			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(1.5f);

			yield return base.IntroSequence(encounter);
			yield return new WaitForSeconds(0.5f);

			yield return base.FaceZoomSequence();
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input
			);
			ViewManager.Instance.SwitchToView(View.Default);
			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
		}

		public override EncounterBlueprintData BuildInitialBlueprint()
		{
			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() { },
				new() { bp_BoneSerpent },
				new() { bp_Skeleton, bp_BoneSerpent },
				new() { },
				new() { },
				new() { bp_Sarcophagus, bp_BoneSerpent },
				new() { },
				new() { bp_Skeleton, bp_BoneSerpent },
				new() { },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_UndeadWolf },
				new() { bp_BoneSerpent, bp_BoneSerpent }
			};

			return blueprint;
		}

		public override IEnumerator StartNewPhaseSequence()
		{
			{
				base.InstantiateBossBehaviour<SawyerBehaviour>();

				var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
				blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
				{
					new() { bp_Skeleton },
					new() { },
					new() { bp_UndeadWolf },
					new() { bp_BoneSerpent },
					new() { },
					new() { },
					new() { },
					new() { bp_BoneSerpent },
					new() { bp_BoneSerpent },
					new() { },
					new() { bp_UndeadWolf },
					new() { bp_BoneSerpent, bp_BoneSerpent }
				};

				yield return ReplaceBlueprintCustom(blueprint);
			}
			yield break;
		}
	}
}