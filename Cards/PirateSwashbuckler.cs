namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateSwashbuckler = "GrimoraMod_PirateSwashbuckler";

	private void Add_PirateSwashbuckler()
	{
		CardBuilder.Builder
			.SetAbilities(Raider.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetNames(NamePirateSwashbuckler, "Swashbuckler")
			.Build();
	}
}
