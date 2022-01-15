using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{
	[HarmonyPatch]
	public class TestingPatches
	{
		[HarmonyPatch(typeof(ViewController))]
		public class ViewControllerPatches
		{
			[HarmonyPrefix, HarmonyPatch(nameof(ViewController.SwitchToControlMode))]
			public static bool Prefix(
				ref ViewController __instance,
				ref ViewController.ControlMode mode,
				bool immediate = false)
			{
				if (!SaveManager.SaveFile.IsGrimora || mode != ViewController.ControlMode.Map)
				{
					return true;
				}

				__instance.controlMode = mode;
				View currentView = ViewManager.Instance.CurrentView;
				__instance.altTransitionInputs.Clear();

				if (mode == ViewController.ControlMode.Map)
				{
					Log.LogDebug($"[ViewController.SwitchToControlMode] Adding MapDeckReview to allowed views");
					__instance.allowedViews = new List<View> { View.MapDefault, View.MapDeckReview };

					if (!__instance.allowedViews.Contains(currentView))
					{
						ViewManager.Instance.SwitchToView(View.MapDefault, immediate);
					}
				}

				return false;
			}
		}


		[HarmonyPatch(typeof(RuleBookController))]
		public class RuleBookControllerPatches
		{
			[HarmonyPrefix, HarmonyPatch(nameof(RuleBookController.Start))]
			public static bool PrefixAddRestOfAbilityMetaCategoriesPatch(ref RuleBookController __instance)
			{
				if (SaveManager.SaveFile.IsGrimora && __instance.PageData is null)
				{
					List<AbilityMetaCategory> pagesToConstruct = new List<AbilityMetaCategory>()
					{
						AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook,
						AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.MagnificusRulebook,
						AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook
					};

					List<RuleBookPageInfo> pageInfos = new List<RuleBookPageInfo>();
					Log.LogDebug($"[RuleBookController.Start] About to start adding all rulebooks");
					foreach (var category in pagesToConstruct)
					{
						pageInfos.AddRange(__instance.bookInfo.ConstructPageData(category));
					}

					pageInfos = pageInfos
						.GroupBy(i => i.ability)
						.Select(i => i.First()).ToList();

					Log.LogDebug($"[RuleBookController.Start] Setting pages of rulebook infos");
					__instance.PageData = pageInfos;

					// RuleBookController.Instance.PageData.ForEach(info => UnityExplorer.ExplorerCore.Log(info.ability));

					// AbilitiesUtil.GetAbilities(true, categoryCriteria: AbilityMetaCategory.Part1Modular)
					// 	.ForEach(ability => UnityExplorer.ExplorerCore.Log(ability));

					return false;
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(GrimoraSaveData))]
		public class GrimoraSaveDataPatches
		{
			private static List<CardInfo> defaultCardInfos = new List<CardInfo>()
			{
				CardLoader.GetCardByName(NameGraveDigger),
				CardLoader.GetCardByName(NameGraveDigger),
				CardLoader.GetCardByName(NameGraveDigger),
				CardLoader.GetCardByName(NameFranknstein),
				CardLoader.GetCardByName(NameFranknstein)
			};

			[HarmonyPrefix, HarmonyPatch(nameof(GrimoraSaveData.Initialize))]
			public static bool PrefixChangeSetupOfGrimoraSaveData(ref GrimoraSaveData __instance)
			{
				__instance.gridX = 0;
				__instance.gridY = 7;
				__instance.removedPieces = new List<int>();
				__instance.deck.Cards.Clear();
				__instance.deck = new DeckInfo();
				foreach (var cardInfo in defaultCardInfos)
				{
					__instance.deck.AddCard(cardInfo);
				}

				return false;
			}
		}
	}
}