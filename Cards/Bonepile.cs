using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonepile = "GrimoraMod_Bonepile";

	private void AddAra_Bonepile()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(1)
			.SetNames(NameBonepile, "Bone Heap")
			.SetDescription("An uninspiring pile of bones. You can have it.")
			.Build()
		);
	}
}
