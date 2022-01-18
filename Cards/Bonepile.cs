using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonepile = "ara_Bonepile";

	private void AddAra_Bonepile()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.QuadrupleBones)
			.WithBaseAttackAndHealth(0, 1)
			.WithBonesCost(1)
			.WithNames(NameBonepile, "Bone Heap")
			.WithDescription("An uninspiring pile of bones. You can have it.")
			.Build()
		);
	}
}