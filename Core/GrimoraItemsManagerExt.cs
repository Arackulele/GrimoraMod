using DiskCardGame;
using UnityEngine;

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
}
