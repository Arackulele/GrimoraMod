using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GrimoraCardChoiceGenerator))]
	public class PlaceAllActOneChoicesToGrimora
	{
		[HarmonyPrefix, HarmonyPatch(nameof(GrimoraCardChoiceGenerator.GenerateChoices))]
		public static bool Prefix(ref List<CardChoice> __result)
		{
			var randomizedChoices = CardLoader
				.AllData
				.FindAll(info => info.metaCategories.Contains(CardMetaCategory.ChoiceNode)
				                 && info.temple == CardTemple.Nature
				)
				.Select(card => new CardChoice { CardInfo = card })
				.ToArray()
				.Randomize()
				.ToList();

			__result = new List<CardChoice> { randomizedChoices[0], randomizedChoices[1], randomizedChoices[2] };
			return false;
		}
	}
}