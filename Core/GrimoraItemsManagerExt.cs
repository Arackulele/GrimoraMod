using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraItemsManagerExt : GrimoraItemsManager
{
	internal HammerItemSlot HammerSlot;
	
	public new static GrimoraItemsManagerExt Instance => ItemsManager.Instance as GrimoraItemsManagerExt;
	
	public override List<string> SaveDataItemsList => Part3SaveData.Data.items;

	public override void OnBattleStart()
	{
		HammerSlot.InitializeHammer();
	}

	public override void OnBattleEnd()
	{
		HammerSlot.CleanupHammer();
	}
}
