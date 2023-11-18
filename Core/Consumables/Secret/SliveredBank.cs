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

public class SliveredBank : ConsumableItem
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


		CustomCoroutine.Instance.StartCoroutine(replaceItem());
	}

	public IEnumerator replaceItem()
	{
		
		Debug.Log("Using SliveredBank2");
		yield return new WaitForSeconds(1f);
		RunState.Run.consumables.Add(GrimoraPlugin.GrimoraItemsSecret.Find(g => g.name.Contains("Slivered Hoggy Bank2")).name);
		Singleton<ItemsManager>.Instance.UpdateItems();
	}

	public static ConsumableItemData NewSliveredBank(GameObject Model)
	{

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Slivered Hoggy Bank", "The Slivered Hoggy bank, gain 2 extra bones, three times in total.", HahaL, typeof(SliveredBank), Model)
		.SetLearnItemDescription("A relic from an age forsaken, this will grant you some extra bones in a pinch.");


		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
