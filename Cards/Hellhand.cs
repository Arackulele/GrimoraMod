using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHellhand = $"{GUID}_Hellhand";

	private void Add_Card_Hellhand()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.LatchBrittle)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(6)
			.SetDescription("I WOULDN'T GET TOO CLOSE DEAR. YOU CAN'T BREAK THE HOLD FROM THE DEEPEST PITS OF HELL, ONCE IT LATCHES ON.")
			.SetNames(NameHellhand, "Hellhand")
			.Build();
	}
}
