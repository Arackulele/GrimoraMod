namespace GrimoraMod;
public partial class GrimoraPlugin
{
	public const string NameTamperedCoffin = $"{GUID}_TamperedCoffin";

	private void Add_Card_TamperedCoffin()
	{
		CardBuilder.Builder
			.SetAbilities(Boneless.ability)
			.SetBaseAttackAndHealth(0, 1)
			// .SetDescription("FOR WHOM THE BELL TOLLS?")
			.SetNames(NameTamperedCoffin, "Tampered Coffin")
			.Build();
	}
}
