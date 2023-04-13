using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraItemsManagerExt : ItemsManager
{
	public new static GrimoraItemsManagerExt Instance => ItemsManager.Instance as GrimoraItemsManagerExt;

	public override List<string> SaveDataItemsList => RunState.Run.consumables;
	public HammerItemSlot HammerSlot => hammerSlot;
	
	private HammerItemSlot hammerSlot;
	
	public void SetHammerActive(bool active = true)
	{
		hammerSlot.gameObject.SetActive(active);
	}

	public override void OnBattleStart()
	{
		// Creates hammer
		hammerSlot.InitializeHammer();
	}

	public override void OnBattleEnd()
	{
		// Deletes hammer
		hammerSlot.CleanupHammer();
	}

	public static void CreateHammerSlot()
	{
		GrimoraItemsManager currentItemsManager = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManager>();

		GrimoraItemsManagerExt ext = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManagerExt>();

		if (ext.SafeIsUnityNull())
		{
			ext = GrimoraItemsManager.Instance.gameObject.AddComponent<GrimoraItemsManagerExt>();
			ext.consumableSlots = currentItemsManager.consumableSlots;
			Destroy(currentItemsManager);

			Part3ItemsManager part3ItemsManager = Instantiate(
				ResourceBank.Get<Part3ItemsManager>("Prefabs/Items/ItemsManager_Part3")
			);

			ext.hammerSlot = part3ItemsManager.hammerSlot;
			Vector3 extentsCopy = ext.hammerSlot.GetComponent<BoxCollider>().extents;
			ext.hammerSlot.GetComponent<BoxCollider>().extents = new Vector3(1f, extentsCopy.y, extentsCopy.z);
			part3ItemsManager.hammerSlot.transform.SetParent(ext.transform);

			float xVal = ConfigHelper.HasIncreaseSlotsMod ? -8.75f : -7.5f;
			ext.hammerSlot.gameObject.transform.localPosition = new Vector3(xVal, 1.25f, -0.48f);
			ext.hammerSlot.gameObject.transform.rotation = Quaternion.Euler(0, 20, -90);
		}

		Part3ItemsManager findObjectOfType = FindObjectOfType<Part3ItemsManager>();
		if (findObjectOfType)
		{
			Destroy(findObjectOfType.gameObject);
		}
		
		/*if (ext.consumableSlots.Any(slot => slot.isActiveAndEnabled))
		{
			// TODO: disable all consumable item slots so the items dont cause a shit load of exceptions
			Log.LogDebug($"[ChangeDefaultHammerModel] Disabling other consumable item slots");
			ext.consumableSlots.ForEach(slot => slot.gameObject.SetActive(false));
		}*/
	}
}

[HarmonyPatch(typeof(ItemSlot))]
public class ItemSlotPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(ItemSlot.CreateItem))]
	public static bool ChangeDefaultHammerModel(ItemSlot __instance, ItemData data, bool skipDropAnimation = false)
	{
		Log.LogDebug($"[ItemSlot.CreateItem] ItemSlot [{__instance}] ItemData [{data}]");
		if (GrimoraSaveUtil.IsGrimoraModRun && data && data.prefabId.Equals("HammerItem"))
		{
			// TODO: Use API to do this instead
			Log.LogDebug($"Adding new HammerItemExt");
			HammerItemExt grimoraHammer = UnityObject.Instantiate(
					AssetConstants.GrimoraHammer,
					__instance.transform
				)
			 .AddComponent<HammerItemExt>();
			Log.LogDebug($"Setting data to old hammer data");
			grimoraHammer.Data = ResourceBank.Get<Item>("Prefabs/Items/" + data.PrefabId).Data;
			__instance.Item = grimoraHammer;
			__instance.Item.SetData(data);
			__instance.Item.PlayEnterAnimation();
		}

		return false;
	}
}

[HarmonyPatch(typeof(ConsumableItemSlot))]
public class DeactivateHammerAfterThreeUses
{
	[HarmonyPostfix, HarmonyPatch(nameof(ConsumableItemSlot.ConsumeItem))]
	public static IEnumerator CheckHammerForThreeUses(IEnumerator enumerator, ConsumableItemSlot __instance)
	{
		yield return enumerator;
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			yield break;
		}

		if (__instance.Consumable is HammerItemExt && HammerItemExt.useCounter==3)
		{
			Log.LogWarning($"Destroying hammer as all 3 uses have been used");
			__instance.coll.enabled = false;
			__instance.gameObject.SetActive(false);
		}
	}
}

[HarmonyPatch(typeof(FirstPersonAnimationController))]
public class FirstPersonHammerPatch
{
	public static bool HasPlayedIceDialogue = false;

	[HarmonyPrefix, HarmonyPatch(nameof(FirstPersonAnimationController.SpawnFirstPersonAnimation))]
	public static bool SpawnFirstPersonHammerGrimora(
		FirstPersonAnimationController __instance,
		ref GameObject __result,
		string prefabName,
		Action<int> keyframesCallback = null
	)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || !prefabName.Contains("FirstPersonHammer"))
		{
			return true;
		}

		if (!HasPlayedIceDialogue
		    && TurnManager.Instance.Opponent is KayceeBossOpponent
		    && BoardManager.Instance.GetPlayerCards().Exists(pCard => pCard.HasAbility(Ability.IceCube))
		   )
		{
			__instance.StartCoroutine(
				TextDisplayer.Instance.ShowThenClear(
					"That hammer doesn't look very sturdy, you'll break it if you bash my ice!",
					3f
				)
			);
			HasPlayedIceDialogue = true;
		}

		Log.LogDebug($"[FirstPersonController] Creating new grimora first person hammer");
		GameObject gameObject = UnityObject.Instantiate(
			AssetConstants.GrimoraFirstPersonHammer,
			__instance.pixelCamera.transform
		);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;

		__result = gameObject;
		return false;
	}
}
