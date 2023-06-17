using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameJikininki = $"{GUID}_Jikininki";

	private void Add_Card_Jikininki()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.OpponentBones)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(5)
			.SetDescription("Overtaken by it's gluttony in life, doomed to it's gluttony in death. Oddly inspires a greed for bones...")
			.SetNames(NameJikininki, "Jikininki")
			.Build();
	}
}
