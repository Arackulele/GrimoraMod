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
			.SetDescription("THEY DIE, OVER AND OVER AGAIN. ONLY FOR THE PHARAOH.")
			.SetNames(NameDeadPets, "Pharaoh's Pets")
			.Build();
	}
}
