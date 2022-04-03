using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardChoiceGenerator))]
public class OnlyAllowGrimoraModCardsInNormalCardChoices
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraCardChoiceGenerator.GenerateChoices))]
	public static bool Prefix(ref List<CardChoice> __result, int randomSeed)
	{
		__result = RandomUtils.GenerateRandomChoicesOfCategory(CardLoader.allData, CardMetaCategory.ChoiceNode);
		return false;
	}
}
