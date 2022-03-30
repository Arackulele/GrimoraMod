using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

// [HarmonyPatch(typeof(ActivatedAbilityBehaviour))]
public class ActivatedAbilityBehaviourPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(ActivatedAbilityBehaviour.CanAfford))]
	public static void CheckForSoulSucker(ActivatedAbilityBehaviour __instance, ref bool __result)
	{
		// int abilityEnergyCost = __instance.EnergyCost;
		// List<CardSlot> soulSuckerSlots = BoardManager.Instance.PlayerSlotsCopy.FindAll(slot => slot.Card && slot.Card.HasAbility(ActivatedGainEnergySoulSucker.ability));
		// GrimoraPlugin.Log.LogDebug($"[CanAfford] abilityEnergyCost [{abilityEnergyCost}] Slots count [{soulSuckerSlots.Count}] Result before [{__result}]");
		// if (!__result && soulSuckerSlots.Any())
		// {
		// 	int energyDiff = abilityEnergyCost - ResourcesManager.Instance.PlayerEnergy;
		// 	int energyToAdd = 0;
		// 	foreach (var card in soulSuckerSlots.Select(slot => slot.Card))
		// 	{
		// 		// if I have 4 energy
		// 		// and soul sucker #1 has 1 energy
		// 		// and soul sucker #2 has 3 energy
		// 		// and I want to play Ember Spirit (6 energy)
		// 		// energy diff is 2
		// 		// then soul sucker #1 should now be zero energy,
		// 		// then soul sucker #2 should now be 2 energy,
		// 		// and I now have 1 energy
		//
		// 		ActivatedGainEnergySoulSucker activatedGainEnergySoulSucker = card.GetComponent<ActivatedGainEnergySoulSucker>();
		// 		// energyToAdd += soulSucker.UseSoulsAndReturnEnergyToAdd(energyDiff);
		// 		if (energyToAdd == energyDiff)
		// 		{			
		// 			ResourcesManager.Instance.PlayerEnergy += energyToAdd;
		// 			ResourceDrone.Instance.UpdateCellAndGemColors();
		// 			__result = true;
		// 			break;
		// 		}
		// 	}
		// }
	}
}
