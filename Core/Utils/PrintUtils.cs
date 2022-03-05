using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrintUtils
{
	public static void PrintAllCards()
	{
		foreach (var info in AllGrimoraModCards)
		{
			string abilities1 = NewAbility.abilities
				.Where(newAb => info.Abilities.Contains(newAb.ability))
				.Select(newAb => newAb.info.rulebookName)
				.Join();
			string abilities2 = NewSpecialAbility.specialAbilities
				.Where(newAb => info.SpecialAbilities.Contains(newAb.specialTriggeredAbility))
				.Select(newAb => newAb.abilityBehaviour.Name)
				.Join();
			string abilities3 = AbilitiesUtil.AllData
				.Where(abInfo => info.Abilities.Contains(abInfo.ability))
				.Select(abInfo => abInfo.rulebookName)
				.Join();
			string joinedAbilities = $"{abilities1},{abilities2},{abilities3}".Trim(',');
			Log.LogDebug(
				$"{info.name}%{info.displayedName}%{info.description}%{info.baseAttack}"
				+ $"%{info.baseHealth}%{info.bonesCost}%{info.energyCost}%{joinedAbilities}"
				+ $"%{info.metaCategories.GetDelimitedString()}"
			);
		}
		
	}
}
