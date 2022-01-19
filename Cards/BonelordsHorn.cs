using APIPlugin;
using DiskCardGame;

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
				.SetAsRareCard()
				.SetAbilities(abilities)
				.SetBaseAttackAndHealth(0, 1)
				.SetEnergyCost(4)
				.SetNames(NameBoneLordsHorn, "Bone Lord's Horn")
				.SetDescription("The Horn of the Bonelord, you do not want to find out what's inside.")
				.Build(),
			iceCubeId: new IceCubeIdentifier(NameBonePrince)
		);
	}
}
