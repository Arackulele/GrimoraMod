using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Fylgja_GuardDog : GuardDog
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
	{
		CardSlot oldSlot = Card.Slot;
		yield return base.OnOtherCardResolve(otherCard);
		yield return SpawnCardOnSlot(oldSlot);
	}

	private IEnumerator SpawnCardOnSlot(CardSlot slot)
	{
		CardInfo cardByName = GrimoraPlugin.NameWardingPresence.GetCardInfo();
		ModifySpawnedCard(cardByName);
		yield return BoardManager.Instance.CreateCardInSlot(cardByName, slot, 0.15f);
	}

	private void ModifySpawnedCard(CardInfo card)
	{
		List<Ability> abilities = Card.Info.Abilities;
		foreach (CardModificationInfo temporaryMod in Card.TemporaryMods)
		{
			abilities.AddRange(temporaryMod.abilities);
		}

		abilities.RemoveAll(x => x == Ability);
		if (abilities.Count > 4)
		{
			abilities.RemoveRange(3, abilities.Count - 4);
		}

		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			fromCardMerge = true,
			abilities = abilities
		};
		card.Mods.Add(cardModificationInfo);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Fylgja_GuardDog()
	{
		const string rulebookDescription =
			"When an opposing creature is placed opposite to an empty space, [creature] will move to that empty space.";

		var ogIcon = AbilityManager.BaseGameAbilities.Single(fb => fb.Id == Ability.GuardDog).Texture;
		ApiUtils.CreateAbility<Fylgja_GuardDog>(rulebookDescription, "Guarding Presence", rulebookIcon: ogIcon);
	}
}
