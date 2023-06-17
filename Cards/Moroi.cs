namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMoroi = $"{GUID}_Moroi";

	private void Add_Card_Moroi()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(ActivatedGainEnergySoulSucker.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(4)
			.SetDescription("Moroi, the hairy ghost. It leeches the souls from those struck down near it.")
			.SetNames(NameMoroi, "Moroi")
			.Build();
	}
}
