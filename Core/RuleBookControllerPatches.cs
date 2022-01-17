using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(RuleBookController))]
public class RuleBookControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(RuleBookController.Start))]
	public static bool PrefixAddRestOfAbilityMetaCategoriesPatch(ref RuleBookController __instance)
	{
		if (SaveManager.SaveFile.IsGrimora && __instance.PageData is null)
		{
			List<AbilityMetaCategory> pagesToConstruct = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook,
				AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.MagnificusRulebook,
				AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook
			};

			List<RuleBookPageInfo> pageInfos = new List<RuleBookPageInfo>();
			Log.LogDebug($"[RuleBookController.Start] About to start adding all rulebooks");
			foreach (var category in pagesToConstruct)
			{
				pageInfos.AddRange(__instance.bookInfo.ConstructPageData(category));
			}

			pageInfos = pageInfos
				.GroupBy(i => i.ability)
				.Select(i => i.First()).ToList();

			Log.LogDebug($"[RuleBookController.Start] Setting pages of rulebook infos");
			__instance.PageData = pageInfos;

			// RuleBookController.Instance.PageData.ForEach(info => UnityExplorer.ExplorerCore.Log(info.ability));

			// AbilitiesUtil.GetAbilities(true, categoryCriteria: AbilityMetaCategory.Part1Modular)
			// 	.ForEach(ability => UnityExplorer.ExplorerCore.Log(ability));

			return false;
		}

		return true;
	}
}