using System;
using System.Collections.Generic;
using System.Reflection;
using APIPlugin;
using DiskCardGame;
using envapitests;
using UnityEngine;

namespace GrimoraMod
{
	public static class ApiUtils
	{
		#region CardUtils

		public static void Add(
			string name, string displayName,
			int baseAttack, int baseHealth,
			string description, int bonesCost,
			byte[] defaultTexture,
			Ability ability = Ability.NUM_ABILITIES, 
			CardMetaCategory metaCategory = CardMetaCategory.NUM_CATEGORIES,
			CardComplexity complexity = CardComplexity.Simple
		)
		{
			Add(
				name, displayName, baseAttack, baseHealth, description,
				bonesCost, defaultTexture,
				new List<Ability>() { ability }, metaCategory, complexity
			);
		}

		public static void Add(
			string name, string displayName,
			int baseAttack, int baseHealth,
			string description, int bonesCost,
			byte[] defaultTexture,
			List<Ability> abilities, 
			CardMetaCategory metaCategory = CardMetaCategory.NUM_CATEGORIES,
			CardComplexity complexity = CardComplexity.Simple
		)
		{
			var metaCategories = new List<CardMetaCategory>();
			
			switch (metaCategory)
			{
				case CardMetaCategory.Rare:
				case CardMetaCategory.GBCPlayable:
					metaCategories.Add(metaCategory);
					break;
				case CardMetaCategory.ChoiceNode:
					metaCategories = CardUtils.getNormalCardMetadata;
					break;
			}

			NewCard.Add(name, displayName, baseAttack, baseHealth, metaCategories,
				complexity, CardTemple.Nature, description, bonesCost: bonesCost,
				abilities: abilities, defaultTex: ImageUtils.LoadTextureFromResource(defaultTexture)
			);
		}

		#endregion

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
				ImageUtils.LoadTextureFromResource(texture),
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
				CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, powerLevel: powerLevel),
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