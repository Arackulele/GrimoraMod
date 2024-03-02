using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePossessedArmour = $"{GUID}_PossessedArmour";

	private void Add_Card_PossessedArmour()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Loot)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(5)
			.SetDescription("A suit of ancient armour in which an occult spirit has taken up residence. Her iron fist drags more creatures to the fight.")
			.SetNames(NamePossessedArmour, "Possessed Armour")
			.Build();
	}
}
