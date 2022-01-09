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
				var cardChoices = new List<CardChoice>();
				foreach (var card in CardLoader.AllData.FindAll(info =>
					info.metaCategories.Contains(CardMetaCategory.ChoiceNode) && info.temple == CardTemple.Nature))
				{
					cardChoices.Add(new CardChoice
					{
						CardInfo = card
					});
				}

				var randomizedChoices = cardChoices.ToArray().Randomize().ToList();
				__result = new List<CardChoice> { randomizedChoices[0], randomizedChoices[1], randomizedChoices[2] };
				return false;
			}
		
	}
}