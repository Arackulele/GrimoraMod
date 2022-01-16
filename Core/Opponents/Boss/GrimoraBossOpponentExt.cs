using System.Collections;
using System.Linq;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public class GrimoraBossOpponentExt : BaseBossExt
	{
		public const string SpecialId = "GrimoraBoss";

		public override StoryEvent EventForDefeat => StoryEvent.PhotoDroneSeenInCabin;

		public override Type Opponent => GrimoraOpponent;

		public override string DefeatedPlayerDialogue => "Thank you!";

		public override int StartingLives => 3;


		public override IEnumerator IntroSequence(EncounterData encounter)
		{
			// Log.LogDebug($"[{GetType()}] Calling base IntroSequence, this creates and sets the candle skull");
			yield return base.IntroSequence(encounter);

			yield return new WaitForSeconds(0.5f);
			// InitializeAudioSources();
			yield return new WaitForSeconds(0.25f);

			AudioController.Instance.SetLoopVolume(1f, 0.5f);
			yield return new WaitForSeconds(1f);

			yield return TextDisplayer.Instance.PlayDialogueEvent("LeshyBossIntro1",
				TextDisplayer.MessageAdvanceMode.Input);
			yield return new WaitForSeconds(0.75f);

			yield return new WaitForSeconds(0.25f);
			yield return TextDisplayer.Instance.PlayDialogueEvent("LeshyBossAddCandle",
				TextDisplayer.MessageAdvanceMode.Input);
			yield return new WaitForSeconds(0.4f);

			bossSkull.EnterHand();
			yield return new WaitForSeconds(3.5f);

			ViewManager.Instance.SwitchToView(View.Default, immediate: false, lockAfter: true);
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
				case 2:
				{
					var playerCardSlots = CardSlotUtils.GetPlayerSlotsWithCards();
					if (playerCardSlots.Count > 0)
					{
						foreach (var playableCard in playerCardSlots.Select(slot => slot.Card))
						{
							TextDisplayer.Instance.ShowUntilInput(
								$"{playableCard.name}, I WILL MAKE YOU WEAK!",
								letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
							);

							playableCard.AddTemporaryMod(
								new CardModificationInfo(
									-playableCard.Attack + 1,
									-playableCard.Health + 1)
							);
							playableCard.Anim.StrongNegationEffect();
							yield return new WaitForSeconds(0.05f);
						}
					}


					break;
				}
			}


			yield break;
		}
	}
}