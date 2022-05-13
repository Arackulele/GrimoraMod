using DiskCardGame;
using InscryptionAPI.Helpers;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(AbilityIconInteractable), "LoadIcon")]
public class LoadSigil_patch
{
	[HarmonyPostfix]
	public static void Postfix(ref Texture __result, ref CardInfo info, ref AbilityInfo ability, ref AbilityIconInteractable __instance)
	{
		if (info.name == GrimoraPlugin.NameOurobones && ability.ability == Ability.Brittle)
		{
			Texture2D Texture;
			Texture = TextureHelper.GetImageAsTexture("ability_CumulativeTorment.png");
			__result = Texture;
		}
		return;
	}
}

public class CumulativeTorment : AbilityBehaviour
{
	public new static Ability ability;

	public override Ability Ability => ability;

}
public partial class GrimoraPlugin
{
	public void Add_Ability_CumulativeTorment()
	{
		const string rulebookDescription =
			"A card bearing this sigil perishes after attacking, then a copy with +1 power and health that costs 1 more bone enters your hand. Resets after every battle.";

		AbilityBuilder<CumulativeTorment>.Builder
		 .SetIcon(TextureHelper.GetImageAsTexture("ability_CumulativeTorment.png"))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Cumulative Torment")
		 .Build();
	}
}

[HarmonyPatch(typeof(RuleBookController), "OpenToAbilityPage")]
public class OpenToAbilityPage_patch
{
	[HarmonyPrefix]
	public static bool OpenToAbilityPage(ref string abilityName, ref PlayableCard card, ref bool immediate)
	{
		Debug.Log("test1");
		Debug.Log("ability: " + abilityName);
		Debug.Log("card: " + (card?.Info?.name ?? "NULL"));
		if (abilityName == Ability.Brittle.ToString() && card?.Info?.name == GrimoraPlugin.NameOurobones)
		{
			Debug.Log("test2");
			Debug.Log(card);
			Debug.Log(card.Info);
			Debug.Log(card.Info.name);

			abilityName = CumulativeTorment.ability.ToString();
		}
		return true;
	}
}
