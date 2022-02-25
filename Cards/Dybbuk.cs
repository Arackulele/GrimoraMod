namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDybbuk = "GrimoraMod_Dybbuk";

	private void Add_Dybbuk()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Possessive.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(3)
			.SetDescription("NO ONE KNOWS WHAT EXACTLY THE DYBBUK IS, SOME SAY IT IS BETTER LEFT UNKNOWN.")
			.SetNames(NameDybbuk, "Dybbuk")
			.Build();
	}
}
