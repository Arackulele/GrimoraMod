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

		if (ext is null)
		{
			Log.LogDebug($"[AddHammer] Creating hammer and GrimoraItemsManagerExt");

			ext = GrimoraItemsManager.Instance.gameObject.AddComponent<GrimoraItemsManagerExt>();
			ext.consumableSlots = currentItemsManager.consumableSlots;
			Log.LogDebug($"[AddHammer] Destroying old manager");
			Destroy(currentItemsManager);

			Part3ItemsManager part3ItemsManager = Instantiate(
				ResourceBank.Get<Part3ItemsManager>("Prefabs/Items/ItemsManager_Part3")
			);

			ext.hammerSlot = part3ItemsManager.hammerSlot;
			Vector3 sizeCopy = ext.hammerSlot.GetComponent<BoxCollider>().size;
			ext.hammerSlot.GetComponent<BoxCollider>().size = new Vector3(1f, sizeCopy.y, sizeCopy.z); 
			part3ItemsManager.hammerSlot.transform.SetParent(ext.transform);

			float xVal = ConfigHelper.Instance.HasIncreaseSlotsMod ? -8.75f : -7.5f;
			ext.hammerSlot.gameObject.transform.localPosition = new Vector3(xVal, 1.25f, -0.48f);
			ext.hammerSlot.gameObject.transform.rotation = Quaternion.Euler(0, 20, -90);
		}

		if (FindObjectOfType<Part3ItemsManager>() is not null)
		{
			Log.LogDebug($"[AddHammer] Destroying existing part3ItemsManager");
			Destroy(FindObjectOfType<Part3ItemsManager>().gameObject);
		}

		Log.LogDebug($"[AddHammer] Finished adding hammer");
	}
}

[HarmonyPatch(typeof(ItemSlot), nameof(ItemSlot.CreateItem))]
public class AddNewHammerExt
{
	[HarmonyPrefix]
	public static bool InitHammerExtAfter(ItemSlot __instance, ItemData data, bool skipDropAnimation = false)
	{
		if (GrimoraSaveUtil.isGrimora && data.prefabId.Equals("HammerItem"))
		{
			if (__instance.Item != null)
			{
				Object.Destroy(__instance.Item.gameObject);
			}

			Log.LogDebug($"Adding new HammerItemExt");
			HammerItemExt grimoraHammer = Object.Instantiate(
				AssetUtils.GetPrefab<GameObject>("GrimoraHammer"),
				__instance.transform
			).AddComponent<HammerItemExt>();
			Log.LogDebug($"Setting data to old hammer data");
			grimoraHammer.Data = ResourceBank.Get<Item>("Prefabs/Items/" + data.PrefabId).Data;
			__instance.Item = grimoraHammer;
			__instance.Item.SetData(data);
			__instance.Item.PlayEnterAnimation();
			return false;
		}

		return true;
	}
}

[HarmonyPatch(typeof(FirstPersonAnimationController), nameof(FirstPersonAnimationController.SpawnFirstPersonAnimation))]
public class FirstPersonHammerPatch
{
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

		Log.LogDebug($"[FirstPersonController] Creating new grimora first person hammer");
		GameObject gameObject = Object.Instantiate(
			AssetUtils.GetPrefab<GameObject>("FirstPersonGrimoraHammer"),
			__instance.pixelCamera.transform
		);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;

		__result = gameObject;
		return false;
	}
}
