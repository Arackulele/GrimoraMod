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
			.SetAbilities(Ability.Deathtouch)
			.SetBaseAttackAndHealth(5, 10)
			.SetBoneCost(6)
			.SetDescription("Lord of Bones, Lord of Bones, answer our call.")
			.SetEnergyCost(6)
			.SetNames(NameBonelord, "The Bone Lord")
			.Build()
		);
	}
}
