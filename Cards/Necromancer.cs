using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNecromancer = $"{GUID}_Necromancer";

	private void Add_Card_Necromancer()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DoubleDeath)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(3)
			.SetDescription("NOTHING DIES ONCE.")
			.SetNames(NameNecromancer, "Necromancer")
			.Build();
	}
}
