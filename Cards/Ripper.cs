using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRipper = "GrimoraMod_Ripper";

	private void Add_Ripper()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(6, 6)
			.SetBoneCost(9)
			.SetNames(NameRipper, "Ripper")
			.SetDescription("WHEN ALL HOPE IS LOST, YOU CAN ALWAYS COUNT ON THIS DEMON STRAIGHT FROM HELL!")
			.Build();
	}
}
