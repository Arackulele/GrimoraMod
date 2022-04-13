using DiskCardGame;

namespace GrimoraMod;

public class CreateShipwrecks : CreateCardsAdjacent
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override string SpawnedCardId => GrimoraPlugin.NameShipwreckDams;

	public override string CannotSpawnDialogue => "Blocked on both sides. No Shipwrecks for the Forgotten Man.";

}

public partial class GrimoraPlugin
{
	public void Add_Ability_CreateShipwrecks()
	{
		const string rulebookDescription =
			$"When [creature] is played, a Shipwreck is created on each empty adjacent space. [define:{NameShipwreckDams}]";

		AbilityBuilder<CreateShipwrecks>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Shipwreck Finder")
		 .Build();
	}
}
