using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNecromancer = "ara_Necromancer";

	private void AddAra_Necromancer()
	{
		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(Ability.DoubleDeath)
			.WithBaseAttackAndHealth(1, 2)
			.WithBonesCost(3)
			.WithDescription("Nothing dies once.")
			.WithNames(NameNecromancer, "Necromancer")
			.Build()
		);
	}
}