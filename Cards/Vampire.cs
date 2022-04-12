namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVampire = $"{GUID}_Vampire";

	private void Add_Card_Vampire()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(BloodGuzzler.ability)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(6)
			.SetDescription("Cursed with eternal life, but only if their thirst can be satisfied.")
			.SetNames(NameVampire, "Forgotten Man")
			.Build();
	}
}
