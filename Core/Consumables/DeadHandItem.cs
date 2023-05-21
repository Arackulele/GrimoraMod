using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using  static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod.Consumables;

public class DeadHandItem : ConsumableItem
{



	public override IEnumerator ActivateSequence()
	{

		Debug.Log("Using Dead Hand");
			ViewManager.Instance.SwitchToView(View.Hand);
			yield return new WaitForSeconds(0.25f);
			List<PlayableCard> cardsNotChoosingASlot = PlayerHand.Instance.CardsInHand.FindAll(x => x != PlayerHand.Instance.ChoosingSlotCard);
			while (cardsNotChoosingASlot.Count > 0)
			{
				cardsNotChoosingASlot[0].SetInteractionEnabled(false);
				cardsNotChoosingASlot[0].Anim.PlayDeathAnimation();
				UnityObject.Destroy(cardsNotChoosingASlot[0].gameObject, 1f);
				PlayerHand.Instance.RemoveCardFromHand(cardsNotChoosingASlot[0]);
				cardsNotChoosingASlot.RemoveAt(0);
			}

			yield return new WaitForSeconds(1f);
		bool drawPile3DIsActive = CardDrawPiles3D.Instance && CardDrawPiles3D.Instance.pile;
		ViewManager.Instance.SwitchToView(View.CardPiles, lockAfter: true);
		yield return new WaitForSeconds(0.75f);
		for (int i = 0; i < 4; i++)
		{
			if (drawPile3DIsActive)
			{
				CardDrawPiles3D.Instance.pile.Draw();
			}

			if (CardDrawPiles3D.Instance.Deck.cards.Count == 0)
			{

				yield return new WaitForSeconds(0.5f);
				ViewManager.Instance.SwitchToView(View.Default);
				yield return CardSpawner.Instance.SpawnCardToHand(NameDeadHand.GetCardInfo());
				ViewManager.Instance.SetViewUnlocked();
				yield break;

			}
			yield return CardDrawPiles.Instance.DrawCardFromDeck();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.5f);
		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.SetViewUnlocked();


	}

	public static ConsumableItemData NewDeadHand(GameObject Model)
	{
		Debug.Log("Added Dead Hand");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Dead Hand", "Draws you a new Hand, at the cost of your old one.", HahaL, typeof(DeadHandItem), Model)
		.SetLearnItemDescription("The severed hand of a forgotten god, left to take on a life of its own. You know what this does.")
		.SetRulebookCategory(AbilityMetaCategory.GrimoraRulebook)
		.SetRulebookName("Dead Hand");
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
