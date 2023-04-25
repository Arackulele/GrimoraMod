using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNosferat = $"{GUID}_Nosferat";

	private void Add_Card_Nosferat()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.LatchBrittle, BloodGuzzler.ability)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(7)
			.SetDescription("The shambling corpse of a pale, sickly noble... it spreads it's filth as it sheds it's flesh.")
			.SetNames(NameNosferat, "Nosferat")
			.Build();
	}
}
