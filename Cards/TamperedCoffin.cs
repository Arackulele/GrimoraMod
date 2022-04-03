namespace GrimoraMod;
public partial class GrimoraPlugin
{
	public const string NameTamperedCoffin = $"{GUID}_TamperedCoffin";

	private void Add_Card_TamperedCoffin()
	{
		CardBuilder.Builder
			.SetAbilities(Boneless.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameTamperedCoffin, "Tampered Coffin")
			.Build();
	}
}
