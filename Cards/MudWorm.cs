using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMudWorm = "ara_MudWorm";

	private void AddAra_RingWorm()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.BoneDigger)
			.WithBaseAttackAndHealth(2, 1)
			.WithBonesCost(5)
			.WithDescription("Like a true worm, loves to dig in the dirt.")
			.WithNames(NameMudWorm, "Mud Worm")
			.WithPortrait(Resources.RingWorm)
			.Build()
		);
	}
}