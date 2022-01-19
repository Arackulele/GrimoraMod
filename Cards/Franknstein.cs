using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFranknstein = "ara_Franknstein";

	private void AddAra_Franknstein()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(5)
			.SetDescription("Best friends, brothers, and fighters.")
			.SetNames(NameFranknstein, "Frank & Stein")
			.Build()
		);
	}
}
