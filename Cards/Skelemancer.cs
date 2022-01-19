using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkelemancer = "ara_Skelemancer";

	private void AddAra_Skelemancer()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(2)
			.SetDescription("Going into that well wasn't the best idea...")
			.SetNames(NameSkelemancer, "Skelemancer")
			.Build()
		);
	}
}
