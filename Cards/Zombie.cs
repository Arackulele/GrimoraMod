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
			.SetDescription("THE HUMBLE ZOMBIE, A RESPECTED MEMBER OF THE ARMY.")
			.SetNames(NameZombie, "Zombie")
			.Build()
		);
	}
}
