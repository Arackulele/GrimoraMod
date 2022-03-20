using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonepile = $"{GUID}_Bonepile";

	private void Add_Bonepile()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(1)
			.SetDescription("AN UNINSPIRING PILE OF BONES. YOU CAN HAVE IT.")
			.SetNames(NameBonepile, "Bone Heap")
			.Build();
	}
}
