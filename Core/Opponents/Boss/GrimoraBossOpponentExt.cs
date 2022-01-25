using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod;

public class GrimoraBossOpponentExt : BaseBossExt
{
	public const string SpecialId = "GrimoraBoss";

	public override StoryEvent EventForDefeat => StoryEvent.PhotoDroneSeenInCabin;

	public override Type Opponent => GrimoraOpponent;

	public override string DefeatedPlayerDialogue => "Thank you!";

	public override int StartingLives => 3;

	private static void SetSceneEffectsShownGrimora()
	{
		Color brightBlue = GameColors.Instance.brightBlue;
		brightBlue.a = 0.5f;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple,
			GameColors.Instance.purple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple,
			GameColors.Instance.purple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple
		);
	}


	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		AudioController.Instance.SetLoopVolume(1f, 0.5f);
		yield return new WaitForSeconds(1f);

		SetSceneEffectsShownGrimora();

		yield return TextDisplayer.Instance.PlayDialogueEvent("LeshyBossIntro1",
			TextDisplayer.MessageAdvanceMode.Input);
		yield return new WaitForSeconds(0.75f);

		// Log.LogDebug($"[{GetType()}] Calling base IntroSequence, this creates and sets the candle skull");
		yield return base.IntroSequence(encounter);

		ViewManager.Instance.SwitchToView(View.BossSkull, immediate: false, lockAfter: true);

		yield return new WaitForSeconds(0.25f);
		yield return TextDisplayer.Instance.PlayDialogueEvent("LeshyBossAddCandle",
			TextDisplayer.MessageAdvanceMode.Input);
		yield return new WaitForSeconds(0.4f);

		bossSkull.EnterHand();
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		switch (this.NumLives)
		{
			case 1:
			{
				GrimoraPlugin.Log.LogDebug($"[GrimoraBoss] Clearing board");
				yield return base.ClearBoard();
				GrimoraPlugin.Log.LogDebug($"[GrimoraBoss] Clearing queue");
				yield return base.ClearQueue();

				yield return new WaitForSeconds(0.5f);

				var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
				blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
				{
					new() { bp_Skeleton },
					new() { bp_Bonelord },
					new() { },
					new() { },
					new() { bp_Draugr, bp_Draugr, bp_Draugr },
					new() { },
					new() { bp_Bonehound, bp_Bonehound },
					new() { },
					new() { bp_Bonehound },
					new() { },
					new() { },
					new() { bp_GhostShip },
					new() { bp_GhostShip }
				};

				var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

				yield return TextDisplayer.Instance.ShowUntilInput("LET THE BONE LORD COMMETH!",
					letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

				yield return BoardManager.Instance.CreateCardInSlot(
					CardLoader.GetCardByName(GrimoraPlugin.NameBonelord), oppSlots[2], 0.2f
				);
				yield return new WaitForSeconds(0.25f);

				oppSlots.RemoveAt(2);

				yield return TextDisplayer.Instance.ShowUntilInput("RISE MY ARMY! RIIIIIIIIIISE!",
					letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

				foreach (CardSlot cardSlot in oppSlots)
				{
					yield return BoardManager.Instance.CreateCardInSlot(
						CardLoader.GetCardByName(GrimoraPlugin.NameSkeletonArmy), cardSlot, 0.2f
					);

					yield return new WaitForSeconds(0.25f);
				}

				break;
			}
			case 2:
			{
				yield return base.ClearBoard();

				var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
				blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
				{
					new() { bp_Sporedigger },
					new() { bp_Poltergeist },
					new() { },
					new() { },
					new() { bp_Draugr, bp_Draugr, bp_Draugr },
					new() { },
					new() { bp_Bonehound, bp_Bonehound },
					new() { },
					new() { bp_Bonehound },
					new() { },
					new() { },
					new() { bp_GhostShip },
					new() { bp_GhostShip },
				};

				var playerCardSlots = CardSlotUtils.GetPlayerSlotsWithCards();
				if (playerCardSlots.Count > 0)
				{
					yield return TextDisplayer.Instance.ShowUntilInput(
						"I WILL MAKE YOU WEAK!",
						letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
					);

					ViewManager.Instance.SwitchToView(View.Board);
					
					foreach (var playableCard in playerCardSlots
						         .Select(slot => slot.Card)
						         .Where(card => card.Health > 1))
					{
						int attack = playableCard.Attack == 0 ? 0 : -playableCard.Attack + 1;
						playableCard.AddTemporaryMod(new CardModificationInfo(attack, -playableCard.Health + 1));
						playableCard.Anim.StrongNegationEffect();
						yield return new WaitForSeconds(0.25f);
						playableCard.Anim.StrongNegationEffect();
					}

					yield return new WaitForSeconds(0.75f);
				}


				break;
			}
		}


		yield break;
	}
}
