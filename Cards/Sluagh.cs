using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSluagh = $"{GUID}_Sluagh";

	private void Add_Card_Sluagh()
	{
		// TODO:  Effect: when played, move any card to any available spot.
		// Does not work on boss cards like the Bone Lord.
		// Cannot move one of Grimora's cards to your side of the board or vice versa.
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(2)
			.SetDescription("THEY SHOW UP OUT OF NOWHERE AND RELOCATE PEOPLE AS THEY PLEASE.")
			.SetNames(NameSluagh, "Sluagh")
			.Build();
	}
}
