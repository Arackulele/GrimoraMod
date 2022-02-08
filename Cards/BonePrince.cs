using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonePrince = "GrimoraMod_BonePrince";

	private void AddAra_BonePrince()
	{
		NewCard.Add(CardBuilder.Builder
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetNames(NameBonePrince, "Bone Prince")
			.SetMetaCategories(CardMetaCategory.GBCPlayable)
			.Build()
		);
	}
}
