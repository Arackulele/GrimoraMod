using APIPlugin;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneSnapper = "ara_BoneSnapper";

	private void AddAra_BoneSnapper()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(1, 6)
			.WithBonesCost(7)
			.WithNames(NameBoneSnapper, "Bone Snapper")
			.WithDescription("One bite of this vile being is strong enough to break it's own shell.")
			.WithPortrait(Resources.BoneSnapper)
			.Build()
		);
	}
}