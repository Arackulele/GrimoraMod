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
		Ability.ActivatedDealDamage, // rethemed as Soul Shot
		Ability.ActivatedDrawSkeleton, // rethemed as Disinter
		Ability.ActivatedRandomPowerBone,
		Ability.ActivatedSacrificeDrawCards,
		Ability.Apparition,
		Ability.BloodGuzzler,
		Ability.BuffGems,
		Ability.CellBuffSelf,
		Ability.CellDrawRandomCardOnDeath,
		Ability.CellTriStrike,
		Ability.ConduitBuffAttack,
		Ability.ConduitEnergy,
		Ability.ConduitFactory,
		Ability.ConduitHeal,
		Ability.ConduitNull,
		Ability.ConduitSpawnGems,
		Ability.CreateDams,
		Ability.CreateEgg,
		Ability.DeleteFile,
		Ability.DrawAnt,
		Ability.DrawVesselOnHit,
		Ability.DropRubyOnDeath,
		Ability.EdaxioArms,
		Ability.EdaxioHead,
		Ability.EdaxioLegs,
		Ability.EdaxioTorso,
		Ability.ExplodeGems,
		Ability.FileSizeDamage,
		Ability.GainBattery,
		Ability.GainGemBlue,
		Ability.GainGemGreen,
		Ability.GainGemOrange,
		Ability.GainGemTriple,
		Ability.GemDependant,
		Ability.GemsDraw,
		Ability.Haunter,
		Ability.HydraEgg,
		Ability.Morsel,
		Ability.PermaDeath,
		Ability.RandomAbility,
		Ability.Sacrificial,
		Ability.ShieldGems,
		Ability.Sinkhole,
		Ability.SquirrelOrbit,
		Ability.SquirrelStrafe,
		Ability.Transformer,
		Ability.TripleBlood,
		Ability.VirtualReality
	};

	[HarmonyAfter(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
	[HarmonyPostfix, HarmonyPatch(nameof(RuleBookInfo.ConstructPageData), typeof(AbilityMetaCategory))]
	public static void PostfixAddRestOfAbilities(
		AbilityMetaCategory metaCategory,
		RuleBookInfo __instance,
		ref List<RuleBookPageInfo> __result
	)
	{
		if (GrimoraSaveUtil.IsNotGrimora)
		{
			return;
		}

		__result.Clear();
		List<int> allAbilities = new List<int>();

		PageRangeInfo pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.Abilities);

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
		int min = allAbilities.AsQueryable().Min();
		int max = allAbilities.AsQueryable().Max() + 1;

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
		allAbilities.Clear();

		allAbilities.AddRange(
			StatIconManager.AllStatIconInfos
			 .Where(info => info && info.rulebookDescription.IsNotEmpty())
			 .Select(info => (int)info.iconType)
			 .ToList()
		);
		min = allAbilities.AsQueryable().Min();
		max = allAbilities.AsQueryable().Max() + 1;

		pageRangeAbilities = __instance.pageRanges.Find(i => i.type == PageRangeType.StatIcons);
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
