using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardSpawner))]
public class CardSpawnerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardSpawner.SpawnPlayableCardWithCopiedMods))]
	public static void CheckForLooseLimb(CardInfo info, PlayableCard copyFrom, ref Ability excludedAbility, ref PlayableCard __result)
	{
		if (GrimoraSaveUtil.IsGrimoraModRun && excludedAbility == Ability.TailOnHit)
		{
			GrimoraPlugin.Log.LogDebug($"[SpawnPlayableCardWithCopiedMods] Changing TailOnHit ability to Loose Limb");
			excludedAbility = LooseLimb.ability;
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(CardSpawner.SpawnPlayableCard))]
	public static void AddCustomAttackPrefabs(CardInfo info, ref PlayableCard __result)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return;
		}

		if (info.Abilities.Count > 4)
		{
			ChangeLogicInCardAbilityIcons.SetupGroupFive(__result.AbilityIcons);
		}

		if (__result.Info.Mods.Exists(mod => mod.fromCardMerge))
		{
			GrimoraPlugin.Log.LogDebug($"[SpawnPlayableCard] Card [{info.displayedName}] has FromCardMerge mod, setting fromCardMerge to false");
			__result.Info.Mods
			 .FindAll(mods => mods.fromCardMerge)
			 .ForEach(mod => mod.fromCardMerge = false);
		}
	}
}
