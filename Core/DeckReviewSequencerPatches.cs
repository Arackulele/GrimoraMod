using System;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(DiskCardGame.DeckReviewSequencer))]
	public class DeckReviewSequencerPatches
	{
		
		[HarmonyPrefix, HarmonyPatch(nameof(DiskCardGame.DeckReviewSequencer.OnEnterDeckView))]
		public static void PrefixOnEnterDeckView()
		{
			GrimoraPlugin.Log.LogDebug($"[DeckReviewSequencer.OnEnterDeckView] OnEnterDeckView called");
		}

		[HarmonyPrefix, HarmonyPatch(nameof(DiskCardGame.DeckReviewSequencer.ArrayDeck))]
		public static void PrefixArrayDeck(List<CardInfo> cards, Action<SelectableCard> selectedCallback = null)
		{
			GrimoraPlugin.Log.LogDebug($"[DeckReviewSequencer.ArrayDeck] Array deck called");
		}
	}
}