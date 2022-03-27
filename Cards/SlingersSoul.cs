namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSlingersSoul = $"{GUID}_SlingersSoul";

	private void Add_SlingersSoul()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(ActivatedDealDamageGrimora.ability)
			.SetBaseAttackAndHealth(2, 3)
			.SetBoneCost(4)
			.SetEnergyCost(3)
			.SetDescription("I WOULDN'T GET TOO CLOSE DEAR. YOU CAN'T BREAK THE HOLD ONCE IT LATCHES ON.")
			.SetNames(NameSlingersSoul, "Slinger's Soul")
			.Build();
	}
}
