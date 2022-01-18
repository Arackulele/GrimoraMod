using APIPlugin;
using DiskCardGame;
using UnityEngine;

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

	internal CardBuilder WithPortrait(string imageFileName)
	{
		_cardInfo.portraitTex = ImageUtils.CreateSpriteFromFile(imageFileName);
		return this;
	}

	internal CardBuilder WithBonesCost(int bonesCost)
	{
		_cardInfo.bonesCost = bonesCost;
		return this;
	}

	internal CardBuilder WithEnergyCost(int energyCost)
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

		return WithDecals(energyDecal);
	}

	internal CardBuilder WithBaseAttackAndHealth(int baseAttack, int baseHealth)
	{
		_cardInfo.baseAttack = baseAttack;
		_cardInfo.baseHealth = baseHealth;
		return this;
	}

	internal CardBuilder WithNames(string name, string displayedName)
	{
		_cardInfo.name = name;
		_cardInfo.displayedName = displayedName;

		return WithPortrait(name);
	}

	internal CardBuilder AsNormalCard()
	{
		return WithMetaCategories(CardUtils.getNormalCardMetadata);
	}

	internal CardBuilder AsRareCard()
	{
		return WithMetaCategory(CardMetaCategory.Rare);
	}

	internal CardBuilder WithDescription(string description)
	{
		_cardInfo.description = description;
		return this;
	}

	internal CardBuilder WithMetaCategory(CardMetaCategory category)
	{
		return WithMetaCategories(new List<CardMetaCategory>() { category });
	}

	internal CardBuilder WithMetaCategories(List<CardMetaCategory> categories)
	{
		_cardInfo.metaCategories = categories;
		return this;
	}

	internal CardBuilder WithAbilities(Ability ability)
	{
		return WithAbilities(new List<Ability>() { ability });
	}

	internal CardBuilder WithAbilities(List<Ability> abilities)
	{
		_cardInfo.abilities = abilities;
		return this;
	}

	internal CardBuilder WithTraits(Trait trait)
	{
		return WithTraits(new List<Trait>() { trait });
	}

	internal CardBuilder WithTraits(List<Trait> traits)
	{
		_cardInfo.traits = traits;
		return this;
	}

	internal CardBuilder WithDecal(Texture decal)
	{
		return WithDecals(new List<Texture>() { decal });
	}

	internal CardBuilder WithDecals(List<Texture> decals)
	{
		_cardInfo.decals = decals;
		return this;
	}
}