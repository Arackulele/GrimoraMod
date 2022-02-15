using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePlagueDoctor = "GrimoraMod_PlagueDoctor";

	private void Add_PlagueDoctor()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(6)
			.SetNames(NamePlagueDoctor, "Plague Doctor")
			.SetDescription("IRONICALLY ENOUGH, NOT A REAL DOCTOR.")
			.Build()
		);
	}
}
