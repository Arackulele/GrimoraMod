using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombie = "ara_Zombie";

	private void AddAra_Zombie()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(2)
			.WithDescription("The humble zombie, a respected member of the army.")
			.WithNames(NameZombie, "Zombie")
			.Build()
		);
	}
}