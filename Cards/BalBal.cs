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
			.SetBoneCost(5)
			.SetDescription("It's foul breath doesn't concern me, it's the Tampered Coffins it leaves behind!")
			.SetNames(NameBalBal, "Bal-Bal")
			.Build();
	}
}
