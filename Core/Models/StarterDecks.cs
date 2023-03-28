using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch]
public class StarterDecks
{
	public static string DefaultStarterDeck { get; private set; }

	private static StarterDeckInfo CreateStarterDeckInfo(string title,  params string[] cards)
	{
		// Texture2D icon = TextureHelper.GetImageAsTexture($"{iconKey}.png", typeof(StarterDecks).Assembly);
		StarterDeckInfo info = ScriptableObject.CreateInstance<StarterDeckInfo>();
		info.name = $"Grimora_{title}";
		info.title = title;
		info.cards = cards.Select(i => i.GetCardInfo()).ToList();
		return info;
	}

	public static List<List<string>> decks = new List<List<string>>()
	{
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameZombie, GrimoraPlugin.NameFranknstein
		},
		new List<string>()
		{
			GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameDybbuk, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameDrownedSoul
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameSilbon, GrimoraPlugin.NameDeathKnell
		},
		new List<string>()
		{
			GrimoraPlugin.NameGhostShip, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameAnimator, GrimoraPlugin.NameTombRobber
		},
	};

	public static List<string> deckNames = new List<string>()
	{
		"Basic",
		"Ghost",
		"Scream",
		"Skeleton",
	};
	
	
	
	

	public static void RegisterStarterDecks()
	{
		for (int i = 0; i < 4; i++)
		{
#if DEBUG
			GrimoraPlugin.Log.LogInfo(i);
#endif
			var d = StarterDeckManager.Add(
				GrimoraPlugin.GUID,
				CreateStarterDeckInfo(deckNames[i], decks[i].ToArray())
			);
			
			d.Info.iconSprite = GrimoraPlugin.AllSprites.Find(o => o.name == "deck_"+deckNames[i].ToLowerInvariant());
			if(i==0)	
				DefaultStarterDeck =d.Info.name;
		}
		
		StarterDeckManager.ModifyDeckList += delegate(List<StarterDeckManager.FullStarterDeck> decks)
		{
			CardTemple acceptableTemple = ScreenManagement.ScreenState;

			// Only keep decks where at least one card belongs to this temple
			decks.RemoveAll(info => info.Info.cards.FirstOrDefault(ci => ci.temple == acceptableTemple) == null);

			return decks;
		};
	}
}
