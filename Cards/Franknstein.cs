using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFranknstein = "ara_Franknstein";

	private void AddAra_Franknstein()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(2, 2)
			.WithBonesCost(5)
			.WithDescription("Best friends, brothers, and fighters.")
			.WithNames(NameFranknstein, "Frank & Stein")
			.Build()
		);
	}
}