using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBooHag = "GrimoraMod_BooHag";

	private void Add_BooHag()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(SkinCrawler.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetNames(NameBooHag, "Boo Hag")
			.SetDescription("When you know shes there, its already too late")
			.Build()
		);
	}
}
