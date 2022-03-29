namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSlingersSoul = $"{GUID}_SlingersSoul";

	private void Add_Card_SlingersSoul()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(ActivatedDealDamageGrimora.ability)
			.SetBaseAttackAndHealth(2, 3)
			.SetBoneCost(4)
			.SetEnergyCost(3)
			.SetDescription("One of the faster draws in the west, but not fast enough...")
			.SetNames(NameSlingersSoul, "Slinger's Soul")
			.Build();
	}
}
