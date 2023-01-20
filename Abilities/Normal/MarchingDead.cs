using System.Collections;
using DiskCardGame;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class MarchingDead : AbilityBehaviour
{
	public const string RulebookName = "Marching Dead";

	public static Ability ability;

	public override Ability Ability => ability;

	public PlayableCard leftAdjCard = null;
	public PlayableCard rightAdjCard = null;

	public override bool RespondsToResolveOnBoard()
	{
		return leftAdjCard || rightAdjCard;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		CardSlot leftAdjSlot = Card.Slot.GetAdjacent(true);
		CardSlot rightAdjSlot = Card.Slot.GetAdjacent(false);
		if (leftAdjCard && leftAdjSlot && leftAdjSlot.Card.SafeIsUnityNull())
		{
			ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
			yield return PlayerHand.Instance.PlayCardOnSlot(leftAdjCard, leftAdjSlot);
			yield return new WaitForSeconds(0.2f);
			leftAdjCard = null;
		}

		if (rightAdjCard && rightAdjSlot && rightAdjSlot.Card.SafeIsUnityNull())
		{
			ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
			yield return PlayerHand.Instance.PlayCardOnSlot(rightAdjCard, rightAdjSlot);
			yield return new WaitForSeconds(0.2f);
			rightAdjCard = null;
		}
		
		ViewManager.Instance.SetViewUnlocked();
	}

	public void SetAdjCardsToPlay(List<PlayableCard> cardsInHand)
	{
		Log.LogDebug($"[MarchingDead] Setting adj cards to play");
		int indexOfCard = cardsInHand.IndexOf(Card);
		for (var i = 0; i < cardsInHand.Count; i++)
		{
			if (i == indexOfCard - 1)
			{
				leftAdjCard = cardsInHand[i];
			}
			else if (i == indexOfCard + 1)
			{
				rightAdjCard = cardsInHand[i];
			}
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_MarchingDead()
	{
		const string rulebookDescription
			= "When [creature] is played, also play the cards in your hand that were adjacent to this card for free.";

		AbilityBuilder<MarchingDead>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(MarchingDead.RulebookName)
		 .Build();
	}
}
