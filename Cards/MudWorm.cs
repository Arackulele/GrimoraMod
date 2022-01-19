using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMudWorm = "ara_MudWorm";

	private void AddAra_RingWorm()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BoneDigger)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			.SetDescription("Like a true worm, loves to dig in the dirt.")
			.SetNames(NameMudWorm, "Mud Worm")
			.Build()
		);
	}
}
