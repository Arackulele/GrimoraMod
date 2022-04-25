namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVampire = $"{GUID}_Vampire";

	private void Add_Card_Vampire()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(BloodGuzzler.ability)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			.SetDescription("Cursed with eternal life, but only if their thirst can be satisfied.")
			.SetNames(NameVampire, "Vampire")
			.Build();
	}
}
