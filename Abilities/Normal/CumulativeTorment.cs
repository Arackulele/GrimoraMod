using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

public class CumulativeTorment : AbilityBehaviour
{
	public const string RulebookName = "Cumulative Torment";

	public static Ability ability;
  
	public override Ability Ability { get => ability; }

}
public partial class GrimoraPlugin
{
	public void Add_Ability_CumulativeTorment()
	{
		const string rulebookDescriptionEnglish =
			"[creature] perishes after attacking, then a copy with +1 power and health that costs 1 more bone enters your hand. Resets after every battle.";
		const string rulebookDescriptionChinese =
			"攻击之后，[creature]会自毁，同时会向你的手牌中添加一张增加1点力量和生命以及多消耗1根骨头的复制品。每场战斗后重置。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<CumulativeTorment>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(CumulativeTorment.RulebookName)
		 .Build();
	}
}

[HarmonyPatch(typeof(AbilityIconInteractable), "LoadIcon")]
public class LoadSigil_patch
{
	[HarmonyPostfix]
	public static void Postfix(ref Texture __result, ref CardInfo info, ref AbilityInfo ability, ref AbilityIconInteractable __instance)
	{
		if (info.name == GrimoraPlugin.NameOurobones && ability.ability == Ability.Brittle)
		{
			 __result = GrimoraPlugin.AllSprites.Find(o => o.name == "ability_tornment").texture;
		}
		return;
	}
}

[HarmonyPatch(typeof(RuleBookController), "OpenToAbilityPage")]
public class OpenToAbilityPage_patch
{
	[HarmonyPrefix]
	public static bool OpenToAbilityPage(ref string abilityName, ref PlayableCard card, ref bool immediate)
	{
		if (abilityName == Ability.Brittle.ToString() && card?.Info?.name == GrimoraPlugin.NameOurobones)
		{
			abilityName = CumulativeTorment.ability.ToString();
		}
		return true;
	}
}
