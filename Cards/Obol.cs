using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameObol = "ara_Obol";

	private void AddAra_Obol()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.Sharp)
			.WithBaseAttackAndHealth(0, 6)
			.WithEnergyCost(3)
			.WithDescription("Going into that well wasn't the best idea...")
			.WithNames(NameObol, "Ancient Obol")
			.WithPortrait(Resources.Obol)
			.Build()
		);
	}
}