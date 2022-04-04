using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraItemsManagerExt : ItemsManager
{
	[SerializeField] internal HammerItemSlot hammerSlot;

	public new static GrimoraItemsManagerExt Instance => ItemsManager.Instance as GrimoraItemsManagerExt;

	public override List<string> SaveDataItemsList => Part3SaveData.Data.items;

	public override void OnBattleStart()
	{
		hammerSlot.InitializeHammer();
	}

	public override void OnBattleEnd()
	{
		hammerSlot.CleanupHammer();
	}

	public static void AddHammer()
	{
		GrimoraItemsManager currentItemsManager = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManager>();

		GrimoraItemsManagerExt ext = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManagerExt>();

		if (ext.IsNull())
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

		if (FindObjectOfType<Part3ItemsManager>())
		{
			Destroy(FindObjectOfType<Part3ItemsManager>().gameObject);
		}
	}
}

[HarmonyPatch(typeof(ItemSlot), nameof(ItemSlot.CreateItem))]
public class AddNewHammerExt
{
	[HarmonyPrefix]
	public static bool InitHammerExtAfter(ItemSlot __instance, ItemData data, bool skipDropAnimation = false)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		if (__instance.Item)
		{
			UnityObject.Destroy(__instance.Item.gameObject);
		}

		if (data.prefabId.Equals("HammerItem"))
		{
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

[HarmonyPatch(typeof(ConsumableItemSlot), nameof(ConsumableItemSlot.ConsumeItem))]
public class DeactivateHammerAfterThreeUses
{
	[HarmonyPostfix]
	public static IEnumerator Postfix(IEnumerator enumerator, ConsumableItemSlot __instance)
	{
		yield return enumerator;
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield break;
		}

		if (__instance.Consumable is HammerItemExt { useCounter: 3 })
		{
			Log.LogDebug($"Destroying hammer as all 3 uses have been used");
			__instance.coll.enabled = false;
			__instance.gameObject.SetActive(false);
		}
	}
}

[HarmonyPatch(typeof(FirstPersonAnimationController), nameof(FirstPersonAnimationController.SpawnFirstPersonAnimation))]
public class FirstPersonHammerPatch
{
	public static bool HasPlayedIceDialogue = false;

	[HarmonyPrefix]
	public static bool InitHammerExtAfter(
		FirstPersonAnimationController __instance,
		ref GameObject __result,
		string prefabName,
		Action<int> keyframesCallback = null
	)
	{
		if (GrimoraSaveUtil.isNotGrimora || !prefabName.Contains("FirstPersonHammer"))
		{
			return true;
		}

		if (!HasPlayedIceDialogue
		    && TurnManager.Instance.Opponent is KayceeBossOpponent
		    && BoardManager.Instance.PlayerSlotsCopy.Exists(slot => slot.CardIsNotNullAndHasAbility(Ability.IceCube))
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
