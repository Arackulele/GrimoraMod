using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonePrince = "GrimoraMod_BonePrince";

	private void Add_BonePrince()
	{
		NewCard.Add(CardBuilder.Builder
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetDescription("MY, WHAT A LOVELY PRINCE!")
			.SetMetaCategories(CardMetaCategory.GBCPlayable)
			.SetNames(NameBonePrince, "Bone Prince")
			.Build()
		);
	}
}
