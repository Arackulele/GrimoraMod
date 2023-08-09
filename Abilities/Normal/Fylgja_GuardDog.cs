using System.Collections;
using DiskCardGame;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod;

public class Fylgja_GuardDog : GuardDog
{
	public const string RulebookName = "Guarding Presence";

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
		yield return slot.CreateCardInSlot(cardByName, 0.15f);
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
		const string rulebookDescriptionEnglish =
			"When an opposing creature is placed opposite to an empty space, [creature] will move to that empty space.";
		const string rulebookDescriptionChinese =
			"如对手的造物对面位置是空的，则[creature]会进入那个位置。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Fylgja_GuardDog>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.GuardDog.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Fylgja_GuardDog.RulebookName)
		 .Build();
	}
}
