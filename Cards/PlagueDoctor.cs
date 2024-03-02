using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePlagueDoctor = $"{GUID}_PlagueDoctor";

	private void Add_Card_PlagueDoctor()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(6)
			.SetNames(NamePlagueDoctor, "Plague Doctor")
			.SetDescription("HE HAS DETERMINED THE UNDEAD ARE SICK WITH A TERRIBLE ILLNESS. THE ONLY CURE IS DEATH.")
			.Build();
	}
}
