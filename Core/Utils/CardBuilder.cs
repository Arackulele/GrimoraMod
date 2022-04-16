using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CardBuilder
{
	public static CardBuilder Builder => new();

	private readonly CardInfo _cardInfo = ScriptableObject.CreateInstance<CardInfo>();

	private string _cardNameNoGuid;

	public CardInfo Build()
	{
		_cardInfo.temple = CardTemple.Undead;

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
		if (ogCardArt.IsNull())
		{
			_cardInfo.SetPortrait(AssetUtils.GetPrefab<Sprite>(_cardNameNoGuid));

			Sprite emissionSprite = AllSprites.Find(_ => _.name.Equals($"{_cardNameNoGuid}_emission"));
			if (emissionSprite)
			{
				_cardInfo.SetEmissivePortrait(emissionSprite);
			}
		}
		else
		{
			Log.LogDebug($"Setting original card art [{ogCardArt.name}]");
			_cardInfo.SetPortrait(ogCardArt);
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

	internal CardBuilder SetTail(CardInfo tailInfo)
	{
		string cardNameNoGuid = _cardInfo.name.Replace($"{GUID}_", string.Empty).Replace("_tail", string.Empty);
		Log.LogDebug($"Setting tail and tailLostPortrait for [{cardNameNoGuid}]");
		Sprite tailLostSprite = AllSprites.Single(_ => _.name.Equals($"{cardNameNoGuid}_tailless"));
		tailLostSprite.RegisterEmissionForSprite(AllSprites.Single(_ => _.name.Equals($"{cardNameNoGuid}_emission")));
		_cardInfo.SetTail(tailInfo, tailLostSprite);
		return this;
	}
	
	internal CardBuilder SetTraits(params Trait[] traits)
	{
		_cardInfo.SetTraits(traits);
		return this;
	}
}
