using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using  static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod.Consumables;

public class SliveredBank3 : ConsumableItem
{




	public override void OnExtraActivationPrerequisitesNotMet()
	{
		base.OnExtraActivationPrerequisitesNotMet();
		this.PlayShakeAnimation();
	}

	public override IEnumerator ActivateSequence()
	{
		Debug.Log("Using SliveredBank");

		yield return ResourcesManager.Instance.AddBones(2);

	}

	public static ConsumableItemData NewSliveredBank(GameObject Model)
	{

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Slivered Hoggy Bank3", "The Slivered Hoggy bank: gain 2 extra bones, one more time.", HahaL, typeof(SliveredBank3), Model)
		.SetLearnItemDescription("A relic from an age forsaken, this will grant you some extra bones in a pinch.");


		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
