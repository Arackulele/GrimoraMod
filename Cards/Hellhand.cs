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
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("I WOULDN'T GET TOO CLOSE DEAR. YOU CAN'T BREAK THE HOLD ONCE IT LATCHES ON. OH, WHAT A LIFE OF SIN HAS BROUGHT YOU.")
			.SetNames(NameHellhand, "Hellhand")
			.Build();
	}
}
