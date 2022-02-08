using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkelemancer = "GrimoraMod_Skelemancer";

	private void AddAra_Skelemancer()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("Coming for Vengence")
			.SetNames(NameSkelemancer, "Vangeful Spirit")
			.Build()
		);
	}
}
