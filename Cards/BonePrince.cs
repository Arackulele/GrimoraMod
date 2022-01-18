using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonePrince = "ara_BonePrince";

	private void AddAra_BonePrince()
	{
		NewCard.Add(CardBuilder.Builder
			.WithBaseAttackAndHealth(2, 1)
			.WithBonesCost(1)
			.WithNames(NameBonePrince, "Bone Prince")
			.WithMetaCategory(CardMetaCategory.GBCPlayable)
			.Build()
		);
	}
}