using APIPlugin;
using DiskCardGame;
using HarmonyLib;
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

			// TODO: refactor when API 2.0 comes out
			AllSpriteAssets.DoIf(
				_ => !NewCard.emissions.ContainsKey(cardName)
				     && _.name.Equals(cardName + "_emission", StringComparison.OrdinalIgnoreCase),
				delegate(Sprite sprite) { NewCard.emissions.Add(cardName, sprite); }
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
		return SetMetaCategories(CardMetaCategory.ChoiceNode, CardMetaCategory.TraderOffer);
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

	internal CardBuilder SetMetaCategories(params CardMetaCategory[] categories)
	{
		_cardInfo.metaCategories = _cardInfo.metaCategories ?? new();
		foreach (var app in categories)
			if (!_cardInfo.metaCategories.Contains(app))
				_cardInfo.metaCategories.Add(app);

		return this;
	}

	internal CardBuilder SetAbilities(params Ability[] abilities)
	{
		_cardInfo.abilities = _cardInfo.abilities ?? new();
		_cardInfo.abilities.AddRange(abilities);

		return this;
	}

	internal CardBuilder SetAbilities(params SpecialTriggeredAbility[] specialTriggeredAbilities)
	{
		_cardInfo.specialAbilities = _cardInfo.specialAbilities ?? new();
		foreach (var ability in specialTriggeredAbilities)
		{
			if (!_cardInfo.specialAbilities.Contains(ability))
			{
				_cardInfo.specialAbilities.Add(ability);
			}
		}

		return this;
	}

	internal CardBuilder SetIceCube(string iceCubeName)
	{
		CardInfo cardToLoad = null;
		try
		{
			cardToLoad = CardLoader.GetCardByName(iceCubeName);
		}
		catch (Exception e)
		{
			cardToLoad = NewCard.cards.Single(_ => _.name.Equals(iceCubeName));
		}
		_cardInfo.iceCubeParams = new()
		{
			creatureWithin = cardToLoad
		};

		return this;
	}

	internal CardBuilder SetEvolve(string evolveInto, int numberOfTurns)
	{
		CardInfo cardToLoad = null;
		try
		{
			cardToLoad = CardLoader.GetCardByName(evolveInto);
		}
		catch (Exception e)
		{
			cardToLoad = NewCard.cards.Single(_ => _.name.Equals(evolveInto));
		}
		
		_cardInfo.evolveParams = new()
		{
			turnsToEvolve = numberOfTurns,
			evolution = cardToLoad
		};
		return this;
	}

	internal CardBuilder SetTraits(params Trait[] traits)
	{
		_cardInfo.traits = _cardInfo.traits ?? new();
		foreach (var trait in traits)
		{
			if (!_cardInfo.traits.Contains(trait))
			{
				_cardInfo.traits.Add(trait);
			}
		}

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
