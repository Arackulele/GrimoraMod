using System.Reflection;
using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{
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
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			return info;
		}

		public static NewAbility CreateAbility<T>(
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
				: AssetUtils.GetPrefab<Texture>("ability_" + typeof(T).Name);
			return CreateAbility<T>(
				CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, activated, flipYIfOpponent, canStack),
				icon
			);
		}

		private static NewAbility CreateAbility<T>(AbilityInfo info, Texture texture) where T : AbilityBehaviour
		{
			Type type = typeof(T);
			// instantiate
			var newAbility = new NewAbility(
				info,
				type,
				texture,
				GetAbilityId(info.rulebookName)
			);

			// Get static field
			FieldInfo field = type.GetField(
				"ability",
				BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
			);
			field.SetValue(null, newAbility.ability);

			return newAbility;
		}

		public static AbilityIdentifier GetAbilityId(string rulebookName)
		{
			return AbilityIdentifier.GetID(GUID, rulebookName);
		}
	}
}
