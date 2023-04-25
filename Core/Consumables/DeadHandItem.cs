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

		List<PlayableCard> list = Singleton<PlayerHand>.Instance.CardsInHand.FindAll((PlayableCard x) => x != Singleton<PlayerHand>.Instance.ChoosingSlotCard);
		while (list.Count > 0)
		{
			list[0].SetInteractionEnabled(interactionEnabled: false);
			list[0].Anim.PlayDeathAnimation();
			UnityEngine.Object.Destroy(list[0].gameObject, 1f);
			Singleton<PlayerHand>.Instance.RemoveCardFromHand(list[0]);
			list.RemoveAt(0);
		}
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < 4; i++)
		{
			yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck();
		}

	}

	public static ConsumableItemData NewDeadHand(GameObject Model)
	{
		Debug.Log("Added Dead Hand");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Dead Hand", "Draws you a new Hand, at the cost of your old one.", HahaL, typeof(DeadHandItem), Model)
		.SetLearnItemDescription("The severed hand of a forgotten god, left to take on a life of its own. You know what this does.");
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
