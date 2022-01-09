using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod
{
	public class RoyalBossExt : BaseBossExt
	{
		
		public override StoryEvent EventForDefeat => StoryEvent.Part3PurchasedHoloBrush;
		
		public override Type Opponent => (Type)1003;

		public override string DefeatedPlayerDialogue => "Arrg! Walk off a Plank yee dirty Scallywag!!";

		public override IEnumerator StartNewPhaseSequence()
		{
			
			GrimoraPlugin.Log.LogDebug($"StartNewPhaseSequence started for RoyalBoss");
			
			var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();

			foreach (var slot in playerSlotsWithCards)
			{
				slot.Card.Anim.PlayDeathAnimation();
			}

			// var blueprint = BuildInitialBlueprint();

			// this.Blueprint = blueprint;

			// List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);

			// this.ReplaceAndAppendTurnPlan(plan);

			yield return this.QueueNewCards();

			yield break;
		}

		public override EncounterBlueprintData BuildInitialBlueprint()
		{
			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() { bp_Skeleton },
				new() { },
				new() { bp_BonePrince },
				new() { bp_Skeleton },
				new() { },
				new() { bp_GhostShip },
				new() { },
				new() { bp_Revenant },
				new() { bp_BonePrince },
				new() { bp_Revenant },
				new() { },
				new() { },
				new() { bp_GhostShip },
				new() { bp_BonePrince },
				new() { },
				new() { bp_BonePrince },
				new() { },
				new() { bp_Revenant }
			};

			return blueprint;
		}
		
		
	}
}