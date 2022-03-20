namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameExplodingPirate = $"{GUID}_ExplodingPirate";

	private void Add_ExplodingPirate()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Anchored.ability, LitFuse.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NameExplodingPirate, "Exploding Pirate")
			.Build();
	}
}
