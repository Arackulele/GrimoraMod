using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(RuleBookInfo))]
public class RulebookInfoPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(RuleBookInfo.ConstructPageData), typeof(AbilityMetaCategory))]
	public static void PostfixAddRestOfAbilities(
		AbilityMetaCategory metaCategory,
		RuleBookInfo __instance,
		ref List<RuleBookPageInfo> __result
	)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return;
		}
		
		List<RuleBookPageInfo> pageInfos = new List<RuleBookPageInfo>();
		List<int> allAbilities = new List<int>();
		
		PageRangeInfo pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.Abilities);
		
		Log.LogDebug($"Start adding abilities");
		List<RuleBookPageInfo> resultCopy = new List<RuleBookPageInfo>(__result);
		allAbilities.AddRange(
			AbilitiesUtil.AllData
				// this is needed because Sinkhole and another ability will throw IndexOutOfBounds exceptions
				.Where(info => info.LocalizedRulebookDescription.IsNotEmpty())
				.Where(info => !resultCopy.Exists(page => page.ability == info.ability))
				.ForEach(
					x =>
					{
						if (x.ability == Ability.DoubleDeath)
						{
							x.rulebookName = "Double Death";
						}
					}
				)
				.Select(x => (int)x.ability)
				.ToList()
		);
		int min = allAbilities.AsQueryable().Min();
		int max = allAbilities.AsQueryable().Max() + 1;

		Log.LogDebug($"Adding abilities to pageInfos");
		pageInfos.AddRange(
			__instance.ConstructPages(
				pageRangeAbilities,
				max,
				min,
				idx => allAbilities.Contains(idx),
				__instance.FillAbilityPage,
				Localization.Translate("APPENDIX XII, SUBSECTION I - ABILITIES {0}")
			)
		);
		
		Log.LogDebug($"Distinct abilities");
		__result = pageInfos
			.GroupBy(i => i.ability)
			.Select(i => i.First())
			.ToList();
	}
}
