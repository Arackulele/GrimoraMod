using System;
using System.Collections.Generic;
using System.Reflection;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public static class ApiUtils
	{
		#region CardUtils

		public static void Add(string name, string displayName,
			string description,
			int baseAttack, int baseHealth,
			int bonesCost,
			byte[] defaultTexture,
			Ability ability = Ability.NUM_ABILITIES,
			CardMetaCategory metaCategory = CardMetaCategory.NUM_CATEGORIES,
			CardComplexity complexity = CardComplexity.Simple,
			List<Texture> decals = null,
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = null,
			IceCubeIdentifier iceCubeId = null,
			EvolveIdentifier evolveId = null,
			List<Trait> traits = null)
		{
			var abilities = new List<Ability>();
			if (ability != Ability.NUM_ABILITIES)
			{
				abilities.Add(ability);
			}
			
			Add(
				name, displayName, description,
				baseAttack, baseHealth,
				bonesCost, defaultTexture, abilities,
				metaCategory, complexity, decals, appearanceBehaviour, iceCubeId, evolveId, traits);
		}

		public static void Add(string name, string displayName,
			string description,
			int baseAttack, int baseHealth,
			int bonesCost,
			byte[] defaultTexture,
			List<Ability> abilities,
			CardMetaCategory metaCategory = CardMetaCategory.NUM_CATEGORIES,
			CardComplexity complexity = CardComplexity.Simple,
			List<Texture> decals = null,
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = null,
			IceCubeIdentifier iceCubeId = null,
			EvolveIdentifier evolveId = null,
			List<Trait> traits = null)
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
			decals ??= new List<Texture>();
			traits ??= new List<Trait>();
			appearanceBehaviour ??= new List<CardAppearanceBehaviour.Appearance>();
			abilities ??= new List<Ability>();

			CardInfo cardInfo = ScriptableObject.CreateInstance<CardInfo>();

			cardInfo.name = name;
			cardInfo.displayedName = displayName;
			cardInfo.baseAttack = baseAttack;
			cardInfo.baseHealth = baseHealth;
			cardInfo.metaCategories = metaCategories;
			cardInfo.cardComplexity = complexity;
			cardInfo.temple = CardTemple.Nature;
			cardInfo.description = description;
			cardInfo.bonesCost = bonesCost;
			cardInfo.abilities = abilities;
			cardInfo.decals = decals;
			cardInfo.appearanceBehaviour = appearanceBehaviour;
			cardInfo.traits = traits;

			var texture = ImageUtils.LoadTextureFromResource(defaultTexture);
			cardInfo.portraitTex = Sprite.Create(
				texture, CardUtils.DefaultCardArtRect, new Vector2(0.5f, 0.65f), 125f
			);

			NewCard.Add(cardInfo, iceCubeId: iceCubeId, evolveId: evolveId);
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