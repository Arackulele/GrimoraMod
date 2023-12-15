using DiskCardGame;

namespace GrimoraMod;

public class CreateShipwrecks : CreateCardsAdjacent
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override string SpawnedCardId => GrimoraPlugin.NameShipwreckDams;

	// TODO: Reconsider this line, and possibly the name of the sigil
	public override string CannotSpawnDialogue => "Blocked on both sides. No Shipwrecks for the Forgotten Man.";

}

public partial class GrimoraPlugin
{
	public void Add_Ability_CreateShipwrecks()
	{
		const string rulebookDescription =
			$"When [creature] is played, a Flotsam is created on each empty adjacent space. [define:{NameShipwreckDams}]";

		AbilityBuilder<CreateShipwrecks>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.CreateDams.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Shipwreck Finder")
		 .Build();
	}
}
