using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;


public static class ApiUtils
{
	public static AbilityInfo CreateInfoWithDefaultSettings(
		string rulebookName,
		string rulebookDescription,
		bool activated,
		bool flipYIfOpponent,
		bool canStack
	)
	{
		AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
		info.activated = activated;
		info.flipYIfOpponent = flipYIfOpponent;
		// Pascal split will make names like "AreaOfEffectStrike" => "Area Of Effect Strike" 
		// "Possessive" => "Possessive" 
		info.rulebookName = rulebookName.Contains(" ")
			                    ? rulebookName
			                    : rulebookName.SplitPascalCase();
		info.rulebookDescription = rulebookDescription;
		info.powerLevel = 0;
		info.canStack = canStack;
		info.metaCategories = new List<AbilityMetaCategory>()
		{
			AbilityMetaCategory.GrimoraRulebook
		};

		return info;
	}

	public static void CreateAbility<T>(
		string rulebookDescription,
		string rulebookName = null,
		bool activated = false,
		Texture rulebookIcon = null,
		bool flipYIfOpponent = false,
		bool canStack = false
	) where T : AbilityBehaviour
	{
		rulebookName ??= typeof(T).Name;
		Texture icon = rulebookIcon
			               ? rulebookIcon
			               : AllAbilityTextures.Single(asset => asset.name.Equals("ability_" + typeof(T).Name)).texture;
		CreateAbility<T>(
			CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, activated, flipYIfOpponent, canStack),
			icon
		);
	}

	private static void CreateAbility<T>(AbilityInfo info, Texture texture)
		where T : AbilityBehaviour
	{
		Type type = typeof(T);
		// instantiate
		var newAbility = AbilityManager.Add(
			GUID,
			info,
			type,
			texture
		);

		// Get static field
		FieldInfo field = type.GetField(
			"ability",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, newAbility.Id);
	}

	public static void CreateSpecialAbility<T>(string nameOfAbility = null) where T : SpecialCardBehaviour
	{
		Type type = typeof(T);
		string finalName = nameOfAbility;
		if (nameOfAbility.IsNullOrWhitespace())
		{
			finalName = type.Name;
		}

		Log.LogDebug($"Starting to add special ability [{type}]");
		var specialAbility = SpecialTriggeredAbilityManager.Add(GUID, finalName, type);

		FieldInfo field = type.GetField(
			"FullSpecial",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, specialAbility);
	}
}
