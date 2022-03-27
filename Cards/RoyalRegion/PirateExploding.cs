namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateExploding = $"{GUID}_ExplodingPirate";

	private void Add_Card_PirateExploding()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Anchored.ability, LitFuse.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NamePirateExploding, "Exploding Pirate")
			.Build();
	}
}
