using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameJikininki = $"{GUID}_Jikininki";

	private void Add_Jikininki()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.OpponentBones)
			.SetBaseAttackAndHealth(0, 2)
			.SetEnergyCost(4)
			.SetDescription("The insatiable Jikininki. Overtaken by it's gluttony in life, doomed to it's gluttony in death.")
			.SetNames(NameJikininki, "Jikininki")
			.Build();
	}
}
