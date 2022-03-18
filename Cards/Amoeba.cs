using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameAmoeba = $"{GUID}_Amoeba";

	private void Add_Amoeba()
	{
		CardInfo cardInfo = "Amoeba".GetCardInfo();
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(GrimoraRandomAbility.ability)
			.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
			.SetBoneCost(cardInfo.bonesCost)
			.SetDescription(cardInfo.description)
			.SetNames(NameAmoeba, cardInfo.displayedName)
			.SetTribes(cardInfo.tribes.ToArray())
			.Build();
	}
}
