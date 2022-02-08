using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneLordsHorn = "GrimoraMod_BonelordsHorn";

	private void Add_BonelordsHorn()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.IceCube, Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(3)
			.SetEnergyCost(2)
			.SetIceCube(NameBonePrince)
			.SetNames(NameBoneLordsHorn, "Bone Lord's Horn")
			.SetDescription("The Horn of the Bonelord, you do not want to find out what's inside.")
			.Build()
		);
	}
}
