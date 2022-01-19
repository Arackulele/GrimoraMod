using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameUndeadWolf = "ara_UndeadWolf";

	private void AddAra_UndeadWolf()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(3, 2)
			.SetBoneCost(4)
			.SetEnergyCost(4)
			.SetDescription("A diseased wolf. The pack has left him for death.")
			.SetNames(NameUndeadWolf, "Undead Wolf")
			.Build()
		);
	}
}
