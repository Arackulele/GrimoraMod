using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadPets = $"{GUID}_DeadPets";

	private void Add_Card_DeadPets()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle, Ability.DrawCopyOnDeath)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(4)
			.SetDescription("FAMED AMONG THE FOLLOWERS OF THE PHARAOH. THEY WERE BLESSED WITH ETERNAL LIFE LONG AGO.")
			.SetNames(NameDeadPets, "Pharaoh's Pets")
			.Build();
	}
}
