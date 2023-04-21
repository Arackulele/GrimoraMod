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

public class Quill : GrimoraTargetSlotItem
{


	public override string FirstPersonPrefabId => null;

	public override Vector3 FirstPersonItemPos => new Vector3(0f, -1f, 2.5f);

	public override Vector3 FirstPersonItemEulers => new Vector3(-60f, 0f, 0f);

	public override View SelectionView => View.OpponentQueue;

	public override CursorType SelectionCursorType => CursorType.Target;

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

		Singleton<ViewManager>.Instance.SwitchToView(View.DefaultUpwards);


		yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target.Card.Info, target.Card.TemporaryMods);


		yield return new WaitForSeconds(0.35f);
	}

	public static ConsumableItemData NewMallet(GameObject Model)
	{
		Debug.Log("Added Quill");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Quill", "Get Quilled.", HahaL, typeof(Quill), Model);
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
