using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHydra = $"{GUID}_Hydra";

	private void Add_Hydra()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DrawCopyOnDeath, Ability.TriStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("DESCRIBED BY SOME AS THE TRUEST NIGHTMARE.")
			.SetNames(NameHydra, "Hydra")
			.Build();
	}
}
