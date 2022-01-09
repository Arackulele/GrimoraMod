using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod
{
	
	
	public class DoggyBossExt : BaseBossExt
	{
		
		public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;
		
		public override Type Opponent => (Type)1002;

		public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

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
				base.InstantiateBossBehaviour<DoggyBehavior>();

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