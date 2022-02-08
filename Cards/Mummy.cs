using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMummy = "GrimoraMod_Mummy";

	private void Add_Mummy()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetDescription("The cycle of the Mummy Lord is never ending.")
			.SetNames(NameMummy, "Mummy Lord")
			.Build()
		);
	}
}
