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

public class BoneHorn : ConsumableItem
{


	public override bool ExtraActivationPrerequisitesMet()
	{
		return (ResourcesManager.Instance.PlayerMaxEnergy > 0);
	}

	public override void OnExtraActivationPrerequisitesNotMet()
	{
		base.OnExtraActivationPrerequisitesNotMet();
		this.PlayShakeAnimation();
	}

	public override IEnumerator ActivateSequence()
	{
		Debug.Log("Using BoneHorn");

		yield return ResourcesManager.Instance.AddBones(ResourcesManager.Instance.PlayerEnergy);
		yield return ResourcesManager.Instance.SpendEnergy(ResourcesManager.Instance.PlayerEnergy);


	}

	public static ConsumableItemData NewBoneHorn(GameObject Model)
	{
		Debug.Log("Added Horn");

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Bone Horn", "Consumes your current Soul, you gain a Bones for each Soul consumed.", HahaL, typeof(BoneHorn), Model)
			.SetLearnItemDescription("Gives you two bones for each Soul you have available, an unfortunate, but worthwhile sacrifice.");
		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
