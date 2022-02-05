﻿using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraItemsManagerExt : ItemsManager
{
	[SerializeField]
	internal HammerItemSlot hammerSlot;
	
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

			
			Part3ItemsManager part3ItemsManager = Instantiate(ResourceBank.Get<Part3ItemsManager>(
					"Prefabs/Items/ItemsManager_Part3"),
				ext.transform,
				true
			);

			ext.hammerSlot = part3ItemsManager.hammerSlot;

			float xVal = Harmony.HasAnyPatches("julianperge.inscryption.act1.increaseCardSlots") ? -8.75f : -7.5f;
			ext.hammerSlot.gameObject.transform.localPosition = new Vector3(xVal, 0.81f, -0.48f);
			ext.hammerSlot.gameObject.transform.rotation = Quaternion.Euler(270f, 315f, 0f);
			
			Log.LogDebug($"[AddHammer] Destroying old manager");
			Destroy(currentItemsManager);
		}

		if (FindObjectOfType<Part3ItemsManager>() is not null)
		{
			Log.LogDebug($"[AddHammer] Destroying existing part3ItemsManager");
			Destroy(FindObjectOfType<Part3ItemsManager>().gameObject);
		}

		Log.LogDebug($"[AddHammer] Finished adding hammer");
	}
}
