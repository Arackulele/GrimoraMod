using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameAmoeba = "GrimoraMod_Amoeba";

	private void Add_Amoeba()
	{
		CardInfo cardInfo = "Amoeba".GetCardInfo();
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(GrimoraRandomAbility.ability)
			.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
			.SetBoneCost(cardInfo.bonesCost)
			.SetDescription(cardInfo.description)
			.SetNames(NameAmoeba, cardInfo.displayedName)
			.SetTribes(cardInfo.tribes.ToArray())
			.Build()
		);
	}
}
