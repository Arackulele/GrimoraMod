using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class CreateShipwrecks : CreateCardsAdjacent
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override string SpawnedCardId => GrimoraPlugin.NameShipwreckDams;

	public override string CannotSpawnDialogue => "Blocked on both sides. No Shipwrecks for the Forgotten Man.";

	private IEnumerator SpawnCardOnSlot(CardSlot slot)
	{
		CardInfo cardByName = CardLoader.GetCardByName(SpawnedCardId);
		yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, slot, 0.15f);
	}


}

public partial class GrimoraPlugin
{
	public void Add_Ability_CreateShipwrecks()
	{
		const string rulebookDescription =
			$"When [creature] is played, a Shipwreck is created on each empty adjacent space. [define:{NameShipwreckDams}]";

		AbilityBuilder<CreateShipwrecks>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.CreateDams.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Shipwreck Finder")
		 .Build();
	}
}
