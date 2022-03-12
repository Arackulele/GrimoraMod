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
		
		List<int> allAbilities = new List<int>();
		
		PageRangeInfo pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.Abilities);
		
		Log.LogDebug($"Start adding abilities");
		allAbilities.AddRange(
			AbilitiesUtil.AllData
				// this is needed because Sinkhole and another ability will throw IndexOutOfBounds exceptions
				.Where(info => info.LocalizedRulebookDescription.IsNotEmpty())
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
		Log.LogDebug($"AllAbilities count [{allAbilities.Count}]");
		int min = allAbilities.AsQueryable().Min();
		int max = allAbilities.AsQueryable().Max() + 1;

		Log.LogDebug($"Adding abilities to pageInfos");
		__result.AddRange(
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
		__result = __result
			.GroupBy(i => i.pageId)
			.Select(i => i.First())
			.ToList();
	}
}
