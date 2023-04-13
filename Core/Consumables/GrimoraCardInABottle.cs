using DiskCardGame;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using UnityEngine;

namespace GrimoraMod.Consumables;

public static class GrimoraCardInABottle
{
	public static ConsumableItemManager.ModelType ModelType;
	
	public static void CreateModel()
	{
		GameObject clone = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/items/FrozenOpossumBottleItem"));
		clone.GetComponent<AssignCardOnStart>().enabled =false;
		
		SelectableCard selectableCard = clone.GetComponentInChildren<SelectableCard>();
		GameObject selectableCardClone = GameObject.Instantiate(AssetConstants.GrimoraSelectableCard, selectableCard.transform.parent);
		selectableCardClone.transform.localRotation = selectableCard.transform.localRotation;
		selectableCardClone.transform.localPosition = selectableCard.transform.localPosition;
		selectableCardClone.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		selectableCard.gameObject.SetActive(false);


		ConsumableItemResource resource = new ConsumableItemResource();
		resource.FromPrefab(clone);
		
		ModelType = ConsumableItemManager.RegisterPrefab(GrimoraPlugin.GUID, "Grimora Card In A Bottle", resource);
		
		GameObject.DontDestroyOnLoad(clone);
	}

	public static ConsumableItemData NewCardBottleItem(string cardName)
	{
		ConsumableItemData data = ConsumableItemManager.NewCardInABottle(GrimoraPlugin.GUID, cardName);
		data.SetPrefabModelType(ModelType);
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
