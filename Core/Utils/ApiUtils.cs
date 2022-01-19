using System.Reflection;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public static class ApiUtils
	{
		#region AbilityUtils

		public static AbilityInfo CreateInfoWithDefaultSettings(
			string rulebookName, string rulebookDescription, int powerLevel = 0
		)
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = powerLevel;
			info.rulebookName = rulebookName;
			info.rulebookDescription = rulebookDescription;
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			return info;
		}

		public static NewAbility CreateAbility<T>(
			byte[] texture,
			string rulebookName,
			string rulebookDescription,
			int powerLevel = 0
		) where T : AbilityBehaviour
		{
			return CreateAbility<T>(
				ImageUtils.LoadTextureFromBytes(texture),
				rulebookName,
				rulebookDescription,
				powerLevel
			);
		}

		public static NewAbility CreateAbility<T>(
			Texture texture,
			string rulebookName,
			string rulebookDescription,
			int powerLevel = 0
		) where T : AbilityBehaviour
		{
			return CreateAbility<T>(
				CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, powerLevel),
				texture
			);
		}

		private static NewAbility CreateAbility<T>(
			AbilityInfo info,
			Texture texture
		) where T : AbilityBehaviour
		{
			Type type = typeof(T);
			// instantiate
			var newAbility = new NewAbility(
				info, type, texture, GetAbilityId(info.rulebookName)
			);

			// Get static field
			FieldInfo field = type.GetField("ability",
				BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
			);
			// GrimoraPlugin.Log.LogDebug($"Setting static field [{field.Name}] for [{type}] with value [{newAbility.ability}]");
			field.SetValue(null, newAbility.ability);

			return newAbility;
		}

		public static AbilityIdentifier GetAbilityId(string rulebookName)
		{
			return AbilityIdentifier.GetID("grimora_test", rulebookName);
		}

		#endregion
	}
}