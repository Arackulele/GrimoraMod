using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePlagueDoctor = "ara_PlagueDoctor";

	private void AddAra_PlagueDoctor()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(6)
			.SetNames(NamePlagueDoctor, "Plague Doctor")
			.SetDescription("Ironically enough not a real doctor.")
			.Build()
		);
	}
}
