using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

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
			.WithDescription("The vicious Necromancer, nothing dies once.")
			.WithNames(NameNecromancer, "Necromancer")
			.WithPortrait(Resources.Necromancer)
			.Build()
		);
	}
}