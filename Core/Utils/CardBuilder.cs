using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CardBuilder
{
	public static CardBuilder Builder => new();

	private readonly CardInfo _cardInfo = ScriptableObject.CreateInstance<CardInfo>();

	private string _cardNameNoGuid;

	private string _cardEmissionNoGuid;


	public CardInfo Build()
	{
		_cardInfo.temple = CardTemple.Undead;

		AllGrimoraModCardsNoGuid.Add(_cardNameNoGuid);
		AllGrimoraModCards.Add(_cardInfo);
		CardManager.Add(GUID, _cardInfo);
		return _cardInfo;
	}

	private CardBuilder()
	{
	}

	internal CardBuilder FlipPortraitForStrafe()
	{
		_cardInfo.flipPortraitForStrafe = true;
		return this;
	}

	internal CardBuilder SetSpecialStatIcon(SpecialStatIcon statIcon)
	{
		_cardInfo.specialStatIcon = statIcon;
		return this;
	}

	internal CardBuilder SetTribes(params Tribe[] tribes)
	{
		_cardInfo.tribes = tribes.ToList();
		return this;
	}

	private CardBuilder SetPortrait(Sprite ogCardArt = null)
	{
		if (ogCardArt.SafeIsUnityNull())
		{
			_cardInfo.portraitTex = AssetUtils.GetPrefab<Sprite>(_cardNameNoGuid);

			Sprite pixelsprite = AllSprites.Find(spr => spr.name.Equals(_cardNameNoGuid+"_pixel"));
			if (pixelsprite)
			{
				_cardInfo.SetPixelPortrait(pixelsprite);
			}

			Sprite emissionSprite = AllSprites.Find(spr => spr.name.Equals(_cardEmissionNoGuid));
			if (emissionSprite)
			{
				_cardInfo.SetEmissivePortrait(emissionSprite);
			}
		}
		else
		{
			_cardInfo.portraitTex = ogCardArt;
		}

		return this;
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
	}

	internal CardBuilder SetBaseAttackAndHealth(int baseAttack, int baseHealth)
	{
		_cardInfo.baseAttack = baseAttack;
		_cardInfo.baseHealth = baseHealth;
		return this;
	}

	internal CardBuilder SetNames(string name, string displayedName, Sprite ogSprite = null)
	{
		_cardNameNoGuid = name.Replace($"{GUID}_", string.Empty);
		_cardEmissionNoGuid = _cardNameNoGuid + "_emission";
		_cardInfo.name = name;
		_cardInfo.displayedName = displayedName;

		return SetPortrait(ogSprite);
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
		if ((categories ?? Array.Empty<CardMetaCategory>()).Contains(CardMetaCategory.Rare))
		{
			_cardInfo.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>
				{ CardAppearanceBehaviour.Appearance.RareCardBackground };
		}
		return this;
	}

	internal CardBuilder SetAbilities(params Ability[] abilities)
	{
		_cardInfo.abilities = abilities.ToList();
		return this;
	}

	internal CardBuilder SetSpecialAbilities(params SpecialTriggeredAbility[] specialTriggeredAbilities)
	{
		_cardInfo.specialAbilities = specialTriggeredAbilities.ToList();
		return this;
	}

	internal CardBuilder SetIceCube(string iceCubeName)
	{
		_cardInfo.SetIceCube(iceCubeName);
		return this;
	}

	internal CardBuilder SetEvolve(string evolveInto, int numberOfTurns)
	{
		_cardInfo.SetEvolve(evolveInto, numberOfTurns);
		return this;
	}

	internal CardBuilder SetTail(string tailName)
	{
		Sprite tailLostSprite = AllSprites.Single(spr => spr.name == $"{_cardNameNoGuid}_tailless");
		tailLostSprite.RegisterEmissionForSprite(AllSprites.Single(spr => spr.name.Equals(_cardEmissionNoGuid)));
		_cardInfo.SetTail(tailName, tailLostSprite);
		return this;
	}
	
	internal CardBuilder SetTraits(params Trait[] traits)
	{
		_cardInfo.SetTraits(traits);

		if ((traits ?? Array.Empty<Trait>()).Contains(Trait.Terrain))
		{
			_cardInfo.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>
				{ CardAppearanceBehaviour.Appearance.TerrainBackground };
		}

		if ((traits ?? Array.Empty<Trait>()).Contains(Trait.DeathcardCreationNonOption))
		{
			_cardInfo.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>
				{ CardAppearanceBehaviour.Appearance.GoldEmission };
		}

		return this;
	}

	internal CardBuilder SetPortraits(Sprite portrait, Sprite portraitm)
	{
		_cardInfo.SetPortrait(portrait);
		_cardInfo.SetEmissivePortrait(portraitm);

		return this;
	}
}
