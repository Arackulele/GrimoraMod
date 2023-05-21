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

public class EmbalmingFluid : GrimoraTargetSlotItem
{



	public override string FirstPersonPrefabId => null;

	public override Vector3 FirstPersonItemPos => new Vector3(0f, -1f, 2.5f);

	public override Vector3 FirstPersonItemEulers => new Vector3(-60f, 0f, 0f);

	public override View SelectionView => View.Board;

	public override CursorType SelectionCursorType => CursorType.Point;

	public override List<CardSlot> GetAllTargets()
	{
		return Singleton<BoardManager>.Instance.PlayerSlotsCopy;
	}

	public override List<CardSlot> GetValidTargets()
	{
		List<CardSlot> PlayerSlotsCopy = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
		PlayerSlotsCopy.RemoveAll((CardSlot x) => x.Card == null || x.Card.Info.HasTrait(Trait.Uncuttable));
		return PlayerSlotsCopy;
	}

	public override IEnumerator OnValidTargetSelected(CardSlot target, GameObject firstPersonItem)
	{

		yield return new WaitForSeconds(0.1f);
				yield return new WaitForSeconds(0.25f);

				CardModificationInfo cardModificationInfo = new CardModificationInfo
				{
					attackAdjustment = 1,
					healthAdjustment = 1,
				};

		target.Card.AddTemporaryMod(cardModificationInfo);

	}

	public static ConsumableItemData NewEmbalmingFluid(GameObject Model)
	{
		Debug.Log("Added Embalming Fluid");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Embalming Fluid", "Pour it over a Card, itll gain 1 attack.", HahaL, typeof(EmbalmingFluid), Model)
		.SetLearnItemDescription("Prepare to put a member of your army to rest, increasing its stamina and vigor as you do so!");
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
