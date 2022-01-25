using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CardBuilder
{
	public static CardBuilder Builder => new();

	private readonly CardInfo _cardInfo = ScriptableObject.CreateInstance<CardInfo>();

	public CardInfo Build()
	{
		if (_cardInfo.metaCategories.Contains(CardMetaCategory.Rare))
		{
			_cardInfo.appearanceBehaviour = CardUtils.getRareAppearance;
		}

		return _cardInfo;
	}

	private CardBuilder()
	{
	}

	internal CardBuilder SetTribes(Tribe tribes)
	{
		return SetTribes(new List<Tribe>() { tribes });
	}

	internal CardBuilder SetTribes(List<Tribe> tribes)
	{
		_cardInfo.tribes = tribes;
		return this;
	}

	private CardBuilder SetPortrait(string cardName, Sprite ogCardArt = null)
	{
		if (ogCardArt is null)
		{
			cardName = cardName.Replace("ara_", "");
			// Log.LogDebug($"Looking in AllSprites for [{cardName}]");
			_cardInfo.portraitTex = AllSpriteAssets.Single(
				spr => string.Equals(spr.name, cardName, StringComparison.OrdinalIgnoreCase)
			);
		}
		else
		{
			Log.LogDebug($"Setting original card art [{ogCardArt.name}]");
			_cardInfo.portraitTex = ogCardArt;
		}

		return this;
	}

	internal CardBuilder SetPortrait(Sprite sprite)
	{
		return SetPortrait(null, sprite);
	}

	internal CardBuilder SetBoneCost(int bonesCost)
	{
		_cardInfo.bonesCost = bonesCost;
		return this;
	}

	internal CardBuilder SetEnergyCost(int energyCost)
	{
		_cardInfo.energyCost = energyCost;
		List<Texture> energyDecal = new();
		switch (energyCost)
		{
			case 1:
				energyDecal.Add(ImageUtils.Energy1);
				break;
			case 2:
				energyDecal.Add(ImageUtils.Energy2);
				break;
			case 3:
				energyDecal.Add(ImageUtils.Energy3);
				break;
			case 4:
				energyDecal.Add(ImageUtils.Energy4);
				break;
			case 5:
				energyDecal.Add(ImageUtils.Energy5);
				break;
			case 6:
				energyDecal.Add(ImageUtils.Energy6);
				break;
		}

		return SetDecals(energyDecal);
	}

	internal CardBuilder SetBaseAttackAndHealth(int baseAttack, int baseHealth)
	{
		_cardInfo.baseAttack = baseAttack;
		_cardInfo.baseHealth = baseHealth;
		return this;
	}

	internal CardBuilder SetNames(string name, string displayedName, Sprite ogSprite = null)
	{
		_cardInfo.name = name;
		_cardInfo.displayedName = displayedName;

		return SetPortrait(name, ogSprite);
	}

	internal CardBuilder SetAsNormalCard()
	{
		return SetMetaCategories(CardUtils.getNormalCardMetadata);
	}

	internal CardBuilder SetAsRareCard()
	{
		return SetMetaCategories(CardMetaCategory.Rare);
	}

	internal CardBuilder SetDescription(string description)
	{
		_cardInfo.description = description;
		return this;
	}

	internal CardBuilder SetMetaCategories(CardMetaCategory category)
	{
		return SetMetaCategories(new List<CardMetaCategory>() { category });
	}

	internal CardBuilder SetMetaCategories(List<CardMetaCategory> categories)
	{
		_cardInfo.metaCategories = categories;
		return this;
	}

	internal CardBuilder SetAbilities(Ability ability)
	{
		return SetAbilities(new List<Ability>() { ability });
	}

	internal CardBuilder SetAbilities(List<Ability> abilities)
	{
		_cardInfo.abilities = abilities;
		return this;
	}

	internal CardBuilder SetTraits(Trait trait)
	{
		return SetTraits(new List<Trait>() { trait });
	}

	internal CardBuilder SetTraits(List<Trait> traits)
	{
		_cardInfo.traits = traits;
		return this;
	}

	internal CardBuilder SetDecals(Texture decal)
	{
		return SetDecals(new List<Texture>() { decal });
	}

	internal CardBuilder SetDecals(List<Texture> decals)
	{
		_cardInfo.decals = decals;
		return this;
	}
}
