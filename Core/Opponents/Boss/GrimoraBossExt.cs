using System;
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

		public override EncounterBlueprintData BuildInitialBlueprint()
		{
			throw new NotImplementedException();
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
					foreach (var oppSlots in BoardManager.Instance.OpponentSlotsCopy)
					{
						yield return BoardManager.Instance.CreateCardInSlot(
							CardLoader.GetCardByName("ara_Bonelord"), oppSlots, 0.2f
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