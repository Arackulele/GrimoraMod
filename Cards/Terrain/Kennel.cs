using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameKennel = $"{GUID}_Kennel";

	private void Add_Card_Kennel()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.IceCube)
			.SetBaseAttackAndHealth(0, 3)
			.SetIceCube(NameBonehound)
			.SetNames(NameKennel, "Kennel")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
