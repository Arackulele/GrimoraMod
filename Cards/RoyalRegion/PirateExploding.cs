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
			.SetDescription("MAYBE IT WASN'T THE BEST IDEA TO HOLD A BARREL FULL OF GUNPOWDER. I'M NOT SURE WETHER HE NOTICED.")
			.SetNames(NamePirateExploding, "Exploding Pirate")
			.Build();
	}
}
