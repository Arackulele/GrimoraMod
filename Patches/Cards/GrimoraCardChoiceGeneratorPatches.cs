using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardChoiceGenerator))]
public class OnlyAllowGrimoraModCardsInNormalCardChoices
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraCardChoiceGenerator.GenerateChoices))]
	public static bool Prefix(ref List<CardChoice> __result, int randomSeed)
	{
		__result = RandomUtils.GenerateRandomChoicesOfCategory(CardManager.AllCardsCopy);
		return false;
	}
}
