namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVengefulSpirit = $"{GUID}_VengefulSpirit";

	private void Add_Card_VengefulSpirit()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("COMING FOR VENGEANCE, AND A BIT OF FUN, TOO!")
			.SetNames(NameVengefulSpirit, "Vengeful Spirit")
			.Build();
	}
}
