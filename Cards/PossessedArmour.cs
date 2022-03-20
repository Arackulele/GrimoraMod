using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePossessedArmour = $"{GUID}_PossessedArmour";

	private static void Add_PossessedArmour()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Loot)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(5)
			.SetDescription("A suit of ancient armour in which a strange spirit has taken up residence. It's iron fist drags more creatures to your hand.")
			.SetNames(NamePossessedArmour, "Possessed Armour")
			.Build();
	}
}
