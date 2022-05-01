using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch]
public class StarterDecks
{
	public static string DefaultStarterDeck { get; private set; }

	private static StarterDeckInfo CreateStarterDeckInfo(string title, string iconKey, params string[] cards)
	{
		// Texture2D icon = TextureHelper.GetImageAsTexture($"{iconKey}.png", typeof(StarterDecks).Assembly);
		StarterDeckInfo info = ScriptableObject.CreateInstance<StarterDeckInfo>();
		info.name = $"Grimora_{title}";
		info.title = title;
		info.iconSprite = Sprite.Create(Texture2D.redTexture, new Rect(0f, 0f, Texture2D.redTexture.width, Texture2D.redTexture.height), new Vector2(0.5f, 0.5f));
		info.cards = cards.Select(i => i.GetCardInfo()).ToList();
		return info;
	}

	public static void RegisterStarterDecks()
	{
		DefaultStarterDeck = StarterDeckManager.Add(
			GrimoraPlugin.GUID, 
			CreateStarterDeckInfo("test", "iconKey", GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger)
		).Info.name;
		
		StarterDeckManager.ModifyDeckList += delegate(List<StarterDeckManager.FullStarterDeck> decks )
		{
			CardTemple acceptableTemple = ScreenManagement.ScreenState;

			// Only keep decks where at least one card belongs to this temple
			decks.RemoveAll(info => info.Info.cards.FirstOrDefault(ci => ci.temple == acceptableTemple) == null);

			return decks;
		};
	}
}
