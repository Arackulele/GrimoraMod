using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWyvern = "ara_Wyvern";

	private void AddAra_Wyvern()
	{
		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(Ability.DrawCopy)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(5)
			.WithDescription("The wyvern army approaches.")
			.WithNames(NameWyvern, "Wyvern")
			.WithPortrait(Resources.Wyvern)
			.Build()
		);
	}
}