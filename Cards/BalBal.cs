using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBalBal = $"{GUID}_BalBal";

	private void Add_Card_BalBal()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BoneThief.ability, Ability.GuardDog)
			.SetSpecialAbilities(FuneralFacade.FullSpecial.Id)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(6)
			.SetDescription("THE REPUGNANT BAL-BAL. IT STEALS CORPSES TO DINE UPON, GIVING IT FOUL BREATH.")
			.SetNames(NameBalBal, "Bal-Bal")
			.Build();
	}
}
