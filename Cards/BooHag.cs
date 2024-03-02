namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBooHag = $"{GUID}_BooHag";

	private void Add_Card_BooHag()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(SkinCrawler.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("SHE FEEDS ON PEOPLES BREATH, MAYBE ONE IS STANDING RIGHT BEHIND YOU RIGHT NOW.")
			.SetNames(NameBooHag, "Boo Hag")
			.Build();
	}
}
