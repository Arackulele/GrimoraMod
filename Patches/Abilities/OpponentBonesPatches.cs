using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;

namespace GrimoraMod;

[HarmonyPatch(typeof(OpponentBones))]
public class OpponentBonesPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(OpponentBones.RespondsToOtherCardDie))]
	public static void CheckForBoneless(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer,
		ref bool __result
	)
	{
		__result = __result && card.LacksAbility(Boneless.ability) && card.LacksTrait(Trait.Terrain);
	}
}
