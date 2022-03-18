using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrintUtils
{
	public static string GetAbilityName(Ability ability)
	{
		string abilityName = ability.ToString();
		if (AbilityManager.AllAbilities.Exists(abInfo => abInfo.Info.ability == ability))
		{
			abilityName = AbilityManager.AllAbilities.Find(abInfo => abInfo.Id == ability).Info.rulebookName;
		}

		Log.LogDebug($"Ability name returning is [{abilityName}]");
		return abilityName;
	}

	public static void PrintAllCards()
	{
		foreach (var info in AllGrimoraModCards)
		{
			string abilities1 = AbilityManager.AllAbilities
				.Where(newAb => info.Abilities.Contains(newAb.Id))
				.Select(newAb => newAb.Info.rulebookName)
				.Join();
			string abilities2 = SpecialTriggeredAbilityManager.AllSpecialTriggers
				.Where(newAb => info.SpecialAbilities.Contains(newAb.Id))
				.Select(newAb => newAb.AbilityBehaviour.Name)
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
