using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRevenant = "ara_Revenant";

	private void AddAra_Revenant()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.Brittle)
			.WithBaseAttackAndHealth(3, 1)
			.WithBonesCost(3)
			.WithDescription("The Revenant, bringing the scythe of death.")
			.WithNames(NameRevenant, "Revenant")
			.Build()
		);
	}
}