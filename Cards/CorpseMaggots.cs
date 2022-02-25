using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCorpseMaggots = "GrimoraMod_Maggots";

	private void Add_CorpseMaggots()
	{
		CardInfo cardInfo = "Maggots".GetCardInfo();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(cardInfo.abilities.ToArray())
			.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
			.SetBoneCost(cardInfo.bonesCost)
			.SetDescription(cardInfo.description)
			.SetNames(NameCorpseMaggots, cardInfo.displayedName)
			.SetTribes(cardInfo.tribes.ToArray())
			.Build();
	}
}
