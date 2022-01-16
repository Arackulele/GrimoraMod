using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{
	public class RoyalBossExt : BaseBossExt
	{
		public GameObject cannons;

		public override StoryEvent EventForDefeat => StoryEvent.Part3PurchasedHoloBrush;

		public override Type Opponent => RoyalOpponent;

		public override string DefeatedPlayerDialogue => "Arrg! Walk off a Plank yee dirty Scallywag!!";

		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			Log.LogDebug($"[{GetType()}] Calling base IntroSequence");
			yield return base.IntroSequence(encounter);

			GrimoraAnimationController.Instance.SetHeadBool("face_happy", val: true);
			yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent(
				"RoyalBossPreIntro",
				TextDisplayer.MessageAdvanceMode.Input
			);
			AudioController.Instance.PlaySound2D("boss_royal");
			yield return new WaitForSeconds(0.1f);

			Log.LogDebug($"[{GetType()}] Calling ShowBossSkull");
			GrimoraAnimationController.Instance.ShowBossSkull();

			Log.LogDebug($"[{GetType()}] Setting Head Trigger");
			GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");

			Log.LogDebug($"[{GetType()}] Setting RoyalBossSkull active");
			RoyalBossSkull.SetActive(true);

			Log.LogDebug($"[{GetType()}] Creating royal mask if not null");
			Mask = RoyalBossSkull;

			Log.LogDebug($"[{GetType()}] Transforming mask");
			Mask.transform.localPosition = new Vector3(0, 0.2f, 0);
			Mask.transform.localRotation = Quaternion.Euler(90, 325, 0);

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

		private static void SetSceneEffectsShownRoyal()
		{
			Color brightBlue = GameColors.Instance.brightBlue;
			brightBlue.a = 0.5f;
			TableVisualEffectsManager.Instance.ChangeTableColors(
				GameColors.Instance.blue,
				GameColors.Instance.marigold,
				GameColors.Instance.brightBlue,
				brightBlue,
				GameColors.Instance.brightBlue,
				GameColors.Instance.brightBlue,
				GameColors.Instance.gray,
				GameColors.Instance.gray,
				GameColors.Instance.lightGray
			);
		}

		public override IEnumerator StartNewPhaseSequence()
		{
			Log.LogDebug($"StartNewPhaseSequence started for RoyalBoss");

			var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();

			// foreach (var slot in playerSlotsWithCards)
			// {
			// 	slot.Card.Anim.PlayDeathAnimation();
			// }

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