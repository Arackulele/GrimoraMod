using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(AbilitiesUtil))]
public class AbilitiesUtilPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AbilitiesUtil.LoadAbilityIcon))]
	public static bool ChangeStupidNegativeYScalingLogWarning(ref Texture __result, string abilityName, bool fillerAbility = false, bool scratched = false)
	{
		if (GrimoraSaveUtil.isNotGrimora || abilityName != Ability.LatchBrittle.ToString() && abilityName != Ability.LatchDeathShield.ToString())
		{
			return true;
		}

		if (abilityName == Ability.LatchBrittle.ToString())
		{
			__result = AssetUtils.GetPrefab<Texture>("ability_LatchBrittle");
		}
		else if (abilityName == Ability.LatchDeathShield.ToString())
		{
			__result = AssetUtils.GetPrefab<Texture>("ability_LatchShield");
		}
		return false;
	}
}
