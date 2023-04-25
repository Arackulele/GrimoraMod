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

public class GrimoraUrn : BatteryItem
{


	public override bool ExtraActivationPrerequisitesMet()
	{
		return (ResourcesManager.Instance.PlayerMaxEnergy < 6
				 || ResourcesManager.Instance.PlayerEnergy < ResourcesManager.Instance.PlayerMaxEnergy);
	}

	public override void OnExtraActivationPrerequisitesNotMet()
	{
		base.OnExtraActivationPrerequisitesNotMet();
		this.PlayShakeAnimation();
	}

	public override IEnumerator ActivateSequence()
	{
		Debug.Log("Using Urn");

		yield return ResourcesManager.Instance.AddEnergy(ResourcesManager.Instance.PlayerMaxEnergy - ResourcesManager.Instance.PlayerEnergy);

	}

	public static ConsumableItemData NewGrimoraUrn(GameObject Model)
	{
		Debug.Log("Added Urn");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Soul Urn", "Contains the Soul of the damned", HahaL, typeof(GrimoraUrn), Model)
		.SetLearnItemDescription("Restores your soul counter to full, the Robot wouldn't think it very useful.");


		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
