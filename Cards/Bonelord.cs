using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = "GrimoraMod_Bonelord";

	private void Add_Bonelord()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BoneLordsReign.ability)
			.SetBaseAttackAndHealth(4, 10)
			.SetBoneCost(17)
			.SetDescription("WHEN THE BONE LORD APPEARS, EVERY CREATURE WILL FALL.")
			.SetNames(NameBonelord, "The Bone Lord")
			.Build()
		);
	}
}
