using APIPlugin;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMummy = "ara_Mummy";

	private void AddAra_Mummy()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(3, 3)
			.WithBonesCost(8)
			.WithDescription("The cycle of the Mummy Lord is never ending.")
			.WithNames(NameMummy, "Mummy Lord")
			.WithPortrait(Resources.Mummy)
			.Build()
		);
	}
}