using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombie = "GrimoraMod_Zombie";

	private void Add_Zombie()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(2)
			.SetDescription("The humble zombie, a respected member of the army.")
			.SetNames(NameZombie, "Zombie")
			.Build()
		);
	}
}
