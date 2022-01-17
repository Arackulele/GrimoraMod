using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneLordsHorn = "ara_BonelordsHorn";

	private void AddAra_BonelordsHorn()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.QuadrupleBones,
			Ability.IceCube
		};

		NewCard.Add(CardBuilder.Builder
				.AsRareCard()
				.WithAbilities(abilities)
				.WithBaseAttackAndHealth(0, 1)
				.WithEnergyCost(4)
				.WithNames(NameBoneLordsHorn, "Bone Lord's Horn")
				.WithDescription("The Horn of the Bonelord, you do not want to find out what's inside.")
				.WithPortrait(Resources.BoneLordsHorn)
				.Build(),
			iceCubeId: new IceCubeIdentifier(NameBonePrince)
		);
	}
}