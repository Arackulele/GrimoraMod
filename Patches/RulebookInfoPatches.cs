using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(RuleBookInfo))]
public class RulebookInfoPatches
{
	private static readonly List<Ability> AbilitiesToRemoveFromRulebook = new()
	{
		Ability.CreateDams,
		Ability.DrawAnt,
		Ability.Sacrificial,
		Ability.TripleBlood,
		Ability.Sinkhole,
		Ability.RandomAbility,
		Ability.SquirrelOrbit,
		Ability.GainGemGreen,
		Ability.GainGemOrange,
		Ability.GainGemBlue,
		Ability.BuffGems,
		Ability.DropRubyOnDeath,
		Ability.GemsDraw,
		Ability.GemDependant,
		Ability.GainGemTriple,
		Ability.SquirrelStrafe,
		Ability.ConduitBuffAttack,
		Ability.ConduitFactory,
		Ability.ConduitHeal,
		Ability.ConduitNull,
		Ability.GainBattery,
		Ability.PermaDeath,
		Ability.FileSizeDamage,
		Ability.DeleteFile,
		Ability.Transformer,
		Ability.ExplodeGems,
		Ability.ShieldGems,
		Ability.DrawVesselOnHit,
		Ability.ConduitEnergy,
		Ability.ActivatedRandomPowerBone,
		Ability.ActivatedDrawSkeleton,
		Ability.ActivatedDealDamage,
		Ability.ConduitSpawnGems,
		Ability.ActivatedSacrificeDrawCards,
		Ability.ActivatedStatsUpEnergy,
		Ability.ActivatedHeal,
		Ability.CellBuffSelf,
		Ability.CellDrawRandomCardOnDeath,
		Ability.CellTriStrike,
		Ability.ActivatedEnergyToBones,
		Ability.BloodGuzzler,
		Ability.Haunter,
		Ability.Apparition,
		Ability.VirtualReality,
		Ability.EdaxioHead,
		Ability.EdaxioArms,
		Ability.EdaxioLegs,
		Ability.EdaxioTorso,
		Ability.CreateEgg,
		Ability.Morsel,
		Ability.HydraEgg
	};

	[HarmonyAfter(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
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

		__result.Clear();
		List<int> allAbilities = new List<int>();

		PageRangeInfo pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.Abilities);

		Log.LogDebug($"Start adding NewSpecialAbilities");
		allAbilities.AddRange(
			AbilityManager.AllAbilityInfos
				// this is needed because Sinkhole and another ability will throw IndexOutOfBounds exceptions
			 .Where(info => info.LocalizedRulebookDescription.IsNotEmpty() && !AbilitiesToRemoveFromRulebook.Contains(info.ability))
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
		Log.LogDebug($"[ConstructPageData] Result after adding custom abilities [{__result.Count}]");

		allAbilities.Clear();

		allAbilities.AddRange(
			StatIconManager.AllStatIconInfos
			 .Where(info => info && info.rulebookDescription.IsNotEmpty())
			 .Select(info => (int)info.iconType)
			 .ToList()
		);
		Log.LogDebug($"SpecialAbilities count [{allAbilities.Join()}]");
		min = allAbilities.AsQueryable().Min();
		max = allAbilities.AsQueryable().Max() + 1;

		pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.StatIcons);
		Log.LogDebug($"Adding special abilities to pageInfos");
		__result.AddRange(
			__instance.ConstructPages(
				pageRangeAbilities,
				max,
				min,
				idx => allAbilities.Contains(idx),
				__instance.FillStatIconPage,
				Localization.Translate("APPENDIX XII, SUBSECTION II - VARIABLE STATS {0}")
			)
		);
	}
}
