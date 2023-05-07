using BepInEx.Bootstrap;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

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
			GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameApparition, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameDrownedSoul
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameDybbuk, GrimoraPlugin.NameDoll, GrimoraPlugin.NameDeathKnell
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameRevenant, GrimoraPlugin.NameAnimator
		},

		// new decks
		new List<string>()
		{
			GrimoraPlugin.NameDraugr, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameTombRobber, GrimoraPlugin.NameFamily, GrimoraPlugin.NameGlacier
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameObol, GrimoraPlugin.NameZombie, GrimoraPlugin.NameBonehound, GrimoraPlugin.NameBonehound
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameSummoner, GrimoraPlugin.NamePirateFirstMateSnag, GrimoraPlugin.NamePirateCaptainYellowbeard
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBoneLordsHorn, GrimoraPlugin.NameGiant, GrimoraPlugin.NameGiant
		},

	};



	public static List<string> deckNames = new List<string>()
	{
		"Basic",
		"Ghost",
		"Scream",
		"Skeleton",
		"Kaycee",
		"Sawyer",
		"Royal",
		"Grimora",
		"Random"
	};
	
	
	
	

	public static void RegisterStarterDecks()
	{
		int amnt = 8;
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			amnt = 9;
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameRandomCard
		});
		}



		for (int i = 0; i < amnt; i++)
		{
#if DEBUG
			GrimoraPlugin.Log.LogInfo(i);
#endif

			var d = StarterDeckManager.Add(
				GrimoraPlugin.GUID,
				CreateStarterDeckInfo(deckNames[i], decks[i].ToArray())
			);

			if (i < 2)
			{
				d.Info.iconSprite = GrimoraPlugin.AllSprites.Find(o => o.name == "deck_" + deckNames[i].ToLowerInvariant());
				if (i == 0)
					DefaultStarterDeck = d.Info.name;

			}

			switch(i)
			{


				default:
				case 0:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("deck_basic");
					break;

				case 1:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("deck_ghost");
					break;

				case 2:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("PSIcon");
					break;


				case 3:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("AMIcon");
					break;

				case 4:
				//d.UnlockLevel = 2;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("KCIcon");
					break;

				case 5:
				//d.UnlockLevel = 3;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("SYIcon");
					break;

				case 6:
				//d.UnlockLevel = 4;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("RYIcon");
					break;

				case 7:
				//d.UnlockLevel = 5;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("GMIcon");
					break;

				case 8:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("RNIcon");
					break;


			}
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
