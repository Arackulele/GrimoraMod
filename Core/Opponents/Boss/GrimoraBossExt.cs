using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public class GrimoraBossExt : BaseBossExt
	{
		public override StoryEvent EventForDefeat => StoryEvent.PhotoDroneSeenInCabin;

		public override Type Opponent => (Type)1004;

		public override string DefeatedPlayerDialogue => "Thank you!";

		public override int StartingLives => 3;


		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			yield return new WaitForSeconds(0.5f);
			// InitializeAudioSources();
			yield return new WaitForSeconds(0.25f);
			AudioController.Instance.SetLoopVolume(1f, 0.5f);
			yield return new WaitForSeconds(1f);
			yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("LeshyBossIntro1",
				TextDisplayer.MessageAdvanceMode.Input);
			yield return new WaitForSeconds(0.75f);
			yield return base.IntroSequence(encounter);
			yield return new WaitForSeconds(0.25f);
			yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("LeshyBossAddCandle",
				TextDisplayer.MessageAdvanceMode.Input);
			yield return new WaitForSeconds(0.4f);
			bossSkull.EnterHand();
			yield return new WaitForSeconds(3.5f);
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, immediate: false, lockAfter: true);
		}

		public override EncounterBlueprintData BuildInitialBlueprint()
		{
			return BlueprintUtils.BuildGrimoraBossRegionBlueprintTwo();
		}

		public override IEnumerator StartNewPhaseSequence()
		{
			switch (this.NumLives)
			{
				case 1:
				{
					var playerCardSlots = BoardManager.Instance.playerSlots.FindAll(slot => slot.Card != null);
					if (playerCardSlots.Count >= 1)
					{
						foreach (var slot in playerCardSlots)
						{
							slot.Card.AddTemporaryMod(new CardModificationInfo(-slot.Card.Attack + 1, -slot.Card.Health + 1));
						}
					}

					break;
				}
				case 2:
				{
					yield return base.ClearBoard();

					var oppSlots = CardSlotUtils.GetOpponentSlotsWithCards();

					TextDisplayer.Instance.ShowUntilInput("LET THE BONE LORD COMMETH!",
						letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

					yield return BoardManager.Instance.CreateCardInSlot(
						CardLoader.GetCardByName(GrimoraPlugin.NameBonelord), oppSlots[2], 0.2f
					);
					yield return new WaitForSeconds(0.25f);

					oppSlots.RemoveAt(2);

					TextDisplayer.Instance.ShowUntilInput("RISE MY ARMY! RIIIIIIIIIISE!",
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
			}


			yield break;
		}
	}
}