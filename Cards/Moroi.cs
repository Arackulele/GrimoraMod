namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMoroi = $"{GUID}_Moroi";

	private void Add_Card_Moroi()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(SoulSucker.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("Moroi, the hairy ghost. It leeches the soul from those it strikes down.")
			.SetNames(NameMoroi, "Moroi")
			.Build();
	}
}
