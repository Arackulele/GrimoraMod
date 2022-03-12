using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGiant : SpecialCardBehaviour
{
	public static SpecialTriggeredAbility SpecialTriggeredAbility;

	public static NewSpecialAbility Create()
	{
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.iconType = SpecialStatIcon.NUM_ICONS;
		info.appliesToAttack = false;
		info.appliesToHealth = false;
		info.iconGraphic = Texture2D.blackTexture;
		info.rulebookName = "Grimora's Giant";
		var sId = SpecialAbilityIdentifier.GetID(GUID, "!GRIMORA_GIANT");

		var ability = new NewSpecialAbility(typeof(GrimoraGiant), sId, info);
		SpecialTriggeredAbility = ability.specialTriggeredAbility;
		return ability;
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		int slotsToSet = 2;
		if (ConfigHelper.HasIncreaseSlotsMod && Card.InfoName().Equals(NameBonelord))
		{
			slotsToSet = 3;
		}

		for (int i = 0; i < slotsToSet; i++)
		{
			BoardManager.Instance.OpponentSlotsCopy[PlayableCard.Slot.Index - i].Card = PlayableCard;
		}

		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		int slotsToSet = 2;
		if (ConfigHelper.HasIncreaseSlotsMod && Card.InfoName().Equals(NameBonelord))
		{
			slotsToSet = 3;
		}

		for (int i = 0; i < slotsToSet; i++)
		{
			BoardManager.Instance.OpponentSlotsCopy[PlayableCard.Slot.Index - i].Card = null;
		}

		yield break;
	}
}
