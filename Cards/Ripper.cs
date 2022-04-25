using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRipper = $"{GUID}_Ripper";

	private void Add_Card_Ripper()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(6, 6)
			.SetBoneCost(6)
			.SetNames(NameRipper, "Ripper")
			.SetDescription("WHEN ALL HOPE IS LOST, YOU CAN ALWAYS COUNT ON THIS DEMON STRAIGHT FROM HELL!")
			.Build();
	}
}
