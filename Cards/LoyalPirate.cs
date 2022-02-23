using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameLoyalPirate = "GrimoraMod_LoyalPirate";
	
	private void Add_LoyalPirate()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LitFuse.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NameLoyalPirate, "Loyal Pirate")
			.Build()
		);
	}
}
