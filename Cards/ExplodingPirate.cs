namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameExplodingPirate = $"{GUID}_ExplodingPirate";

	private void Add_ExplodingPirate()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LitFuse.ability, SeaLegs.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NameExplodingPirate, "Exploding Pirate")
			.Build();
	}
}
