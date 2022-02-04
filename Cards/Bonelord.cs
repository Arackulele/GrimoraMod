using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = "ara_Bonelord";

	private void AddAra_Bonelord()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BoneLordsReign.ability)
			.SetBaseAttackAndHealth(4, 10)
			.SetBoneCost(8)
			.SetDescription("Lord of Bones, Lord of Bones, answer our call.")
			.SetEnergyCost(6)
			.SetNames(NameBonelord, "The Bone Lord")
			.Build()
		);
	}
}
