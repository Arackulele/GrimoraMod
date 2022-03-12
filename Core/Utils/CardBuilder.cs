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

		_cardInfo.temple = CardTemple.Undead;

		AllGrimoraModCards.Add(_cardInfo);
		NewCard.Add(_cardInfo);
		return _cardInfo;
	}

	private CardBuilder()
	{
	}

	internal CardBuilder SetSpecialStatIcon(SpecialStatIcon statIcon)
	{
		_cardInfo.specialStatIcon = statIcon;
		return this;
	}

	internal CardBuilder SetTribes(params Tribe[] tribes)
	{
		_cardInfo.tribes = tribes?.ToList();
		return this;
	}

	private CardBuilder SetPortrait(string cardName, Sprite ogCardArt = null)
	{
		if (ogCardArt.IsNull())
		{
			cardName = cardName.Replace("GrimoraMod_", "");
			// Log.LogDebug($"Looking in AllSprites for [{cardName}]");
			_cardInfo.portraitTex = AssetUtils.GetPrefab<Sprite>(cardName);

			// TODO: refactor when API 2.0 comes out
			AllSprites.DoIf(
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
		return this;
		// Texture energyDecal = energyCost switch
		// {
		// 	1 => ImageUtils.Energy1,
		// 	2 => ImageUtils.Energy2,
		// 	3 => ImageUtils.Energy3,
		// 	4 => ImageUtils.Energy4,
		// 	5 => ImageUtils.Energy5,
		// 	6 => ImageUtils.Energy6,
		// 	_ => null
		// };
		//
		// return SetDecals(energyDecal);
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

	internal CardBuilder SetAppearance(params CardAppearanceBehaviour.Appearance[] appearance)
	{
		_cardInfo.appearanceBehaviour = appearance.ToList();
		return this;
	}

	internal CardBuilder SetMetaCategories(params CardMetaCategory[] categories)
	{
		_cardInfo.metaCategories = categories?.ToList();
		return this;
	}

	internal CardBuilder SetAbilities(params Ability[] abilities)
	{
		_cardInfo.abilities = abilities?.ToList();
		return this;
	}

	internal CardBuilder SetAbilities(params SpecialTriggeredAbility[] specialTriggeredAbilities)
	{
		_cardInfo.specialAbilities = specialTriggeredAbilities?.ToList();
		return this;
	}

	internal CardBuilder SetIceCube(string iceCubeName)
	{
		CardInfo cardToLoad = null;
		try
		{
			cardToLoad = iceCubeName.GetCardInfo();
		}
		catch (Exception e)
		{
			cardToLoad = NewCard.cards.Single(_ => _.name.Equals(iceCubeName));
		}

		_cardInfo.iceCubeParams = new IceCubeParams
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
			cardToLoad = evolveInto.GetCardInfo();
		}
		catch (Exception e)
		{
			cardToLoad = NewCard.cards.Single(_ => _.name.Equals(evolveInto));
		}

		_cardInfo.evolveParams = new EvolveParams
		{
			turnsToEvolve = numberOfTurns,
			evolution = cardToLoad
		};
		return this;
	}

	internal CardBuilder SetTraits(params Trait[] traits)
	{
		_cardInfo.traits = traits?.ToList();
		return this;
	}

	internal CardBuilder SetDecals(params Texture[] decals)
	{
		_cardInfo.decals ??= new List<Texture>();
		_cardInfo.decals = decals.ToList();
		return this;
	}
}
