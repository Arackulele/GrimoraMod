namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateSwashbuckler = $"{GUID}_PirateSwashbuckler";

	private void Add_PirateSwashbuckler()
	{
		CardBuilder.Builder
			.SetAbilities(Raider.ability, SeaLegs.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetNames(NamePirateSwashbuckler, "Swashbuckler")
			.Build();
	}
}
