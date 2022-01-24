using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SawyerBossOpponent : BaseBossExt
{
	public const string SpecialId = "SawyerBoss";

	public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;

	public override Type Opponent => SawyerOpponent;

	public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead");
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead", 1);
		base.SpawnScenery("CratesTableEffects");
		yield return new WaitForSeconds(0.5f);
		// TurnManager.Instance.Opponent.NumTurnsTaken;
		// TurnManager.Instance.Opponent.TurnPlan;

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);

		SetSceneEffectsShownSawyer();

		yield return base.IntroSequence(encounter);
		yield return new WaitForSeconds(0.5f);

		yield return base.FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"Look away, Look away! If you want to fight, get it over quick!", -0.65f, 0.4f, Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);

		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	private static void SetSceneEffectsShownSawyer()
	{
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.darkGold,
			GameColors.Instance.orange,
			GameColors.Instance.yellow,
			GameColors.Instance.yellow,
			GameColors.Instance.orange,
			GameColors.Instance.yellow,
			GameColors.Instance.brown,
			GameColors.Instance.orange,
			GameColors.Instance.brown
		);
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		{
			base.InstantiateBossBehaviour<SawyerBehaviour>();

			yield return base.FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"Please, I don't want to fight anymore! Get it over with!",
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true
			);
			yield return this.ClearBoard();
			var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
			foreach (var playerSlot in playerSlotsWithCards)
			{
				// card.SetIsOpponentCard();
				// card.transform.eulerAngles += new Vector3(0f, 0f, -180f);
				yield return BoardManager.Instance.CreateCardInSlot(
					CardLoader.GetCardByName("ara_Obol"), playerSlot.opposingSlot, 0.25f
				);
			}


			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() {  },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_Bonehound },
				new() { },
				new() { },
				new() { bp_BoneSerpent },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_Sarcophagus },
				new() { bp_BoneSerpent, bp_BoneSerpent }
			};

			yield return ReplaceBlueprintCustom(blueprint);
		}
		yield break;
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"Thanks for getting it over with, and don't ever return!",
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true
			);

			yield return new WaitForSeconds(0.5f);
			yield return base.OutroSequence(true);

			yield return base.FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"The next area won't be so easy. I asked Royal to do his best at making it impossible.",
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true);
		}
		else
		{
			Log.LogDebug($"[{GetType()}] Defeated player dialogue");
			yield return TextDisplayer.Instance.ShowUntilInput(
				DefeatedPlayerDialogue,
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true
			);
		}


		yield break;
	}
}
