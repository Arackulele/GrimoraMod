using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod.Consumables;

public class BoneLordSkull : ConsumableItem
{
	public override IEnumerator ActivateSequence()
	{
		PlayExitAnimation();
		yield return new WaitForSeconds(0.25f);
		yield return ResourcesManager.Instance.AddBones(8);
	}
}
