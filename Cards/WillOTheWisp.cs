using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWillOTheWisp = $"{GUID}_WillOTheWisp";

	private void Add_WillOTheWisp()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(SpiritBearer.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(1)
			.SetDescription("THE WHISPERING WILL 'O' THE WISP. ITS PRESENCE WILL GRANT YOU AN ADDITIONAL SOUL.")
			.SetNames(NameWillOTheWisp, "Will 'O' The Wisp")
			.Build();
	}
}
