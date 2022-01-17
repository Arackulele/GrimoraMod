using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod;

public class KayceeBossOpponent : BaseBossExt
{
	public const string SpecialId = "kayceeBoss";

	public override StoryEvent EventForDefeat => StoryEvent.FactoryConveyorBeltMoved;

	public override Type Opponent => KayceeOpponent;

	public override string DefeatedPlayerDialogue => "Youuuuuuu, painnnfulllll deaaathhh awaiiitttsss youuuuuuu!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead");
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead", 1);
		yield return new WaitForSeconds(0.5f);


		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);
		SetSceneEffectsShownKaycee();

		yield return base.IntroSequence(encounter);
		yield return new WaitForSeconds(0.5f);

		yield return base.FaceZoomSequence();
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"Brrrr!Ive been freezing for ages! Lets turn up the Heat in a good fight!", -0.65f, 0.4f, Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);

		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	private static void SetSceneEffectsShownKaycee()
	{
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.darkBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.nearWhite,
			GameColors.Instance.blue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue
		);
	}

	public override EncounterBlueprintData BuildInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Zombie },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new() { },
			new() { bp_Skeleton },
			new() { bp_Revenant },
			new() { },
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Skeleton, bp_Draugr },
			new() { },
			new() { },
			new() { bp_Revenant },
			new() { bp_Skeleton },
			new() { bp_DrownedSoul },
			new() { bp_Revenant }
		};

		return blueprint;
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_DrownedSoul },
			new() { bp_Skeleton },
			new() { bp_Draugr },
			new() { },
			new() { bp_Zombie },
			new() { },
			new() { bp_Zombie },
			new() { },
			new() { },
			new() { bp_Draugr },
			new() { bp_Zombie },
			new() { },
			new() { },
			new() { bp_HeadlessHorseman },
			new() { bp_Draugr }
		};

		return blueprint;
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		{
			yield return base.FaceZoomSequence();
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Im still not feeling Warmer!", -0.65f, 0.4f,
				Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);

			yield return this.ClearBoard();
			var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
			foreach (var playerSlot in playerSlotsWithCards)
			{
				// card.SetIsOpponentCard();
				// card.transform.eulerAngles += new Vector3(0f, 0f, -180f);
				yield return BoardManager.Instance.CreateCardInSlot(
					playerSlot.Card.Info, playerSlot.opposingSlot, 0.25f
				);
			}

			yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		}
		yield break;
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"Oh come on dude,Im still Cold! Lets fight again soon!", -0.65f, 0.4f, Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
		TableVisualEffectsManager.Instance.ResetTableColors();

		yield return new WaitForSeconds(1f);

		yield return base.FaceZoomSequence();
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"This next Area was made by Sawyer, one of my Ghouls. He says it is terrible.", -0.65f, 0.4f, Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);


		yield break;
	}
}