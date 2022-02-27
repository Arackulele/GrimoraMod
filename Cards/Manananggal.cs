using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameManananggal = "GrimoraMod_Manananggal";

	private void Add_Manananggal()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying)
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetDescription("THEY HAVE LEARNED THE ANCIENT SPELL OF DEATH.")
			.SetNames(NameManananggal, "Manananggal")
			.Build();
	}
}

