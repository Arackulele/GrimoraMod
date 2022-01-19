using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneSnapper = "ara_BoneSnapper";

	private void AddAra_BoneSnapper()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 6)
			.SetBoneCost(7)
			.SetNames(NameBoneSnapper, "Bone Snapper")
			.SetDescription("One bite of this vile being is strong enough to break it's own shell.")
			.Build()
		);
	}
}
