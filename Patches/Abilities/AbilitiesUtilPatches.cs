using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(AbilitiesUtil))]
public class AbilitiesUtilPatches
{
	private static readonly Dictionary<string, Texture> UpdatedLatchIcons = new()
	{
		{ Ability.LatchBrittle.ToString(), AssetUtils.GetPrefab<Texture>("ability_LatchBrittle") },
		{ Ability.LatchDeathShield.ToString(), AssetUtils.GetPrefab<Texture>("ability_LatchShield") },
		{ Ability.LatchExplodeOnDeath.ToString(), AssetUtils.GetPrefab<Texture>("ability_LatchBomb") },
	};

	[HarmonyPrefix, HarmonyPatch(nameof(AbilitiesUtil.LoadAbilityIcon))]
	public static bool ChangeStupidNegativeYScalingLogWarning(ref Texture __result, string abilityName, bool fillerAbility = false, bool scratched = false)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || !UpdatedLatchIcons.ContainsKey(abilityName))
		{
			return true;
		}

		__result = UpdatedLatchIcons.GetValueSafe(abilityName);
		return false;
	}
}
