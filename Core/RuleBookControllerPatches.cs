using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(RuleBookController))]
public class RuleBookControllerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(RuleBookController.Start))]
	public static void AddAllAbilitiesPatch(ref RuleBookController __instance)
	{
		if (SaveManager.SaveFile.IsGrimora)
		{
			List<RuleBookPageInfo> pageInfos = new List<RuleBookPageInfo>();
			Log.LogDebug($"[RuleBookController.Start] About to start adding all rulebooks");
			List<int> abilitiesNoCategory = AbilitiesUtil.AllData
				// this is needed because Sinkhole and another ability will throw IndexOutOfBounds exceptions
				.Where(info => !string.IsNullOrEmpty(info.LocalizedRulebookDescription))
				.Select(x => (int)x.ability).ToList();
			int min = abilitiesNoCategory.AsQueryable().Min();
			int max = abilitiesNoCategory.AsQueryable().Max();
			PageRangeInfo pageRange = __instance.bookInfo.pageRanges.Find(i => i.type == PageRangeType.Abilities);

			bool DoAddPageFunc(int index) => abilitiesNoCategory.Contains(index);

			pageInfos.AddRange(
				__instance.bookInfo.ConstructPages(
					pageRange,
					max + 1,
					min,
					DoAddPageFunc,
					__instance.bookInfo.FillAbilityPage,
					Localization.Translate("APPENDIX XII, SUBSECTION I - ABILITIES {0}")
				)
			);

			pageInfos = pageInfos
				.GroupBy(i => i.ability)
				.Select(i => i.First()).ToList();

			Log.LogDebug($"[RuleBookController.Start] Setting pages of rulebook infos. Total [{pageInfos.Count}]");
			__instance.PageData = pageInfos;

			// RuleBookController.Instance.PageData.ForEach(info => UnityExplorer.ExplorerCore.Log(info.ability));

			// AbilitiesUtil.GetAbilities(true, categoryCriteria: AbilityMetaCategory.Part1Modular)
			// 	.ForEach(ability => UnityExplorer.ExplorerCore.Log(ability));
		}
	}
}
