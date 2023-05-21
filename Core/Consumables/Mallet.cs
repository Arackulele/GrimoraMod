using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Card;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using  static GrimoraMod.GrimoraPlugin;
namespace GrimoraMod.Consumables;

public class Mallet : GrimoraTargetSlotItem
{


	public override string FirstPersonPrefabId => null;

	public override Vector3 FirstPersonItemPos => new Vector3(0f, -1f, 2.5f);

	public override Vector3 FirstPersonItemEulers => new Vector3(-60f, 0f, 0f);

	public override View SelectionView => View.OpponentQueue;

	public override CursorType SelectionCursorType => CursorType.Hammer;

	public override List<CardSlot> GetAllTargets()
	{
		return Singleton<BoardManager>.Instance.OpponentSlotsCopy;
	}

	public override List<CardSlot> GetValidTargets()
	{
		List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
		opponentSlotsCopy.RemoveAll((CardSlot x) => x.Card == null || x.Card.Info.HasTrait(Trait.Uncuttable));
		return opponentSlotsCopy;
	}


	public override IEnumerator OnValidTargetSelected(CardSlot target, GameObject firstPersonItem)
	{


		yield return new WaitForSeconds(0.1f);
			yield return target.Card.TakeDamage(1, null);
			if (target.Card != null)
			{
				yield return new WaitForSeconds(0.25f);

				CardModificationInfo cardModificationInfo = new CardModificationInfo
				{
					abilities = new List<Ability> { Ability.Brittle },
				};

				target.Card.AddTemporaryMod(cardModificationInfo);


				}
		yield return new WaitForSeconds(0.35f);
	}

	public static ConsumableItemData NewMallet(GameObject Model)
	{
		Debug.Log("Added Mallet");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Mallet", "Whack a Card, itll become brittle and loose 1 Hp.", HahaL, typeof(Mallet), Model)
		.SetLearnItemDescription("You'd think this is comical, but it is quite cruel. Injures a card, and leaves it frail.");
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
