using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayableCard))]
public class PlayableCardPatches
{
	// [HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.CanPlay))]
	public static void CanPlayCheckSoulSucker(PlayableCard __instance, ref bool __result)
	{
		// List<CardSlot> soulSuckerSlots = BoardManager.Instance.PlayerSlotsCopy.FindAll(slot => slot.Card && slot.Card.HasAbility(ActivatedGainEnergySoulSucker.ability));
		// int energyCost = __instance.EnergyCost;
		// bool doesNotHaveEnoughEnergy = __instance.Info.BonesCost <= ResourcesManager.Instance.PlayerBones
		//                             && energyCost > ResourcesManager.Instance.PlayerEnergy;
		// if (!__result && doesNotHaveEnoughEnergy && soulSuckerSlots.Any())
		// {
		// 	Log.LogDebug($"[CanPlay] result is false and does not have enough energy. Energy Cost [{energyCost}]");
		// 	int energyDiff = energyCost - ResourcesManager.Instance.PlayerEnergy;
		// 	int energyToAdd = 0;
		// 	foreach (var suckerCard in soulSuckerSlots.Select(slot => slot.Card))
		// 	{
		// 		// if I have 3 energy
		// 		// and soul sucker has 2 energy
		// 		// and I want to play Skelemagus
		// 		// then soul sucker should now be zero energy, and I now have zero energy 
		//
		// 		// if I have 4 energy
		// 		// and soul sucker has 2 energy
		// 		// and I want to play Skelemagus
		// 		// then soul sucker should now be zero energy, and I now have 1 energy
		// 		// ActivatedGainEnergySoulSucker activatedGainEnergySoulSucker = suckerCard.GetComponent<ActivatedGainEnergySoulSucker>();
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

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.Die))]
	public static IEnumerator ExtendDieMethod(
		IEnumerator enumerator,
		PlayableCard __instance,
		bool wasSacrifice,
		PlayableCard killer = null,
		bool playSound = true
	)
	{
		yield return __instance.DieCustom(wasSacrifice, killer, playSound);
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void PossessiveGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.Slot.opposingSlot.CardIsNotNullAndHasAbility(Possessive.ability))
		{
			var adjSlots = BoardManager.Instance
			 .GetAdjacentSlots(__instance.Slot)
			 .Where(_ => _.Card)
			 .ToList();

			__result = new List<CardSlot>();
			if (adjSlots.IsNotEmpty())
			{
				CardSlot slotToTarget = adjSlots[UnityEngine.Random.Range(0, adjSlots.Count)];
				// Log.LogDebug($"[OpposingPatches.Possessive] Slot targeted for attack [{slotToTarget.Index}]");
				__result.Add(slotToTarget);
			}
		}
	}

	[HarmonyPrefix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static bool CorrectBuffsAndDebuffsForGrimoraGiants(PlayableCard __instance, ref int __result)
	{
		bool isGrimoraGiant = __instance.Info.HasTrait(Trait.Giant) && __instance.HasSpecialAbility(GrimoraGiant.FullSpecial.Id);
		if (__instance.OnBoard && isGrimoraGiant)
		{
			int finalAttackNum = 0;
			List<CardSlot> opposingSlots = BoardManager.Instance.GetSlots(__instance.OpponentCard).Where(slot => slot.Card).ToList();
			foreach (var opposingSlot in opposingSlots)
			{
				if (opposingSlot.Card.HasAbility(Ability.BuffEnemy))
				{
					finalAttackNum++;
				}

				if (!__instance.HasAbility(Ability.MadeOfStone) && opposingSlot.Card.HasAbility(Ability.DebuffEnemy))
				{
					finalAttackNum--;
				}
			}

			List<CardSlot> slotsWithGiants = BoardManager.Instance.GetSlots(!__instance.OpponentCard).Where(slot => slot.Card == __instance).ToList();
			foreach (var giant in slotsWithGiants)
			{
				List<CardSlot> adjSlotsWithCards = BoardManager.Instance.GetAdjacentSlots(giant).Where(slot => slot && slot.Card && slot.Card != __instance).ToList();
				if (adjSlotsWithCards.Exists(slot => slot.Card.HasAbility(Ability.BuffNeighbours)))
				{
					finalAttackNum++;
				}
			}

			__result = finalAttackNum;
			return false;
		}

		return true;
	}
}
