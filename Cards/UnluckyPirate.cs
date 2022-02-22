using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameUnluckyPirate = "GrimoraMod_UnluckyPirate";
	
	private void Add_UnluckyPirate()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LitFuse.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(2)
			.SetDescription("THAT'S WHAT HAPPENS WHEN YOU PLAY WITH BOMBS!")
			.SetNames(NameUnluckyPirate, "Unlucky Pirate")
			.Build()
		);
	}
}
