namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFylgja = $"{GUID}_Fylgja";

	private void Add_Card_Fylgja()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Fylgja_GuardDog.ability)
			.SetBaseAttackAndHealth(2, 3)
			.SetBoneCost(7)
			.SetDescription("A ghastly guardian spirit. It's presence lingers behind as it sprints through the dark.")
			.SetNames(NameFylgja, "Fylgja")
			.Build();
	}
}
