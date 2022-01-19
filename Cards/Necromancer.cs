using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNecromancer = "ara_Necromancer";

	private void AddAra_Necromancer()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DoubleDeath)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(3)
			.SetDescription("Nothing dies once.")
			.SetNames(NameNecromancer, "Necromancer")
			.Build()
		);
	}
}
