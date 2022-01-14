﻿using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod
{
	public class RoyalBossExt : BaseBossExt
	{
		public GameObject cannons;

		public override StoryEvent EventForDefeat => StoryEvent.Part3PurchasedHoloBrush;

		public override Type Opponent => (Type)1003;

		public override string DefeatedPlayerDialogue => "Arrg! Walk off a Plank yee dirty Scallywag!!";

		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			yield return base.IntroSequence(encounter);

			GrimoraAnimationController.Instance.SetHeadBool("face_happy", val: true);
			yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("RoyalBossPreIntro",
				TextDisplayer.MessageAdvanceMode.Input);
			AudioController.Instance.PlaySound2D("boss_royal");
			yield return new WaitForSeconds(0.1f);

			// GrimoraAnimationController.Instance.ShowBossSkull();
			// GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
			yield return new WaitForSeconds(0.05f);

			Singleton<ViewManager>.Instance.SwitchToView(View.BossCloseup, immediate: false, lockAfter: true);
			SetSceneEffectsShownRoyal();
			yield return new WaitForSeconds(1f);

			StartCoroutine(Singleton<TextDisplayer>.Instance.ShowThenClear("Y'AARRRRR!", 1.5f));
			Singleton<ViewManager>.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(0.75f);

			cannons = Object.Instantiate(
				ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/CannonTableEffects")
			);
			yield return new WaitForSeconds(2f);
		}

		private void SetSceneEffectsShownRoyal()
		{
			Color brightBlue = GameColors.Instance.brightBlue;
			brightBlue.a = 0.5f;
			Singleton<TableVisualEffectsManager>.Instance.ChangeTableColors(GameColors.Instance.blue,
				GameColors.Instance.marigold, GameColors.Instance.brightBlue, brightBlue, GameColors.Instance.brightBlue,
				GameColors.Instance.brightBlue, GameColors.Instance.gray, GameColors.Instance.gray,
				GameColors.Instance.lightGray);
		}

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