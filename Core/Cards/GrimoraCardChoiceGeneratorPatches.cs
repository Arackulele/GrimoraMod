using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardChoiceGenerator))]
public class OnlyAllowGrimoraModCardsInNormalCardChoices
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraCardChoiceGenerator.GenerateChoices))]
	public static bool Prefix(ref List<CardChoice> __result, ref int randomSeed)
	{
		__result = RandomUtils.GenerateRandomChoicesOfCategory(randomSeed);
		return false;
	}
}
