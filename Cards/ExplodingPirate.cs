namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameExplodingPirate = "GrimoraMod_ExplodingPirate";
	
	private void Add_ExplodingPirate()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LitFuse.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NameExplodingPirate, "Exploding Pirate")
			.Build();
	}
}
