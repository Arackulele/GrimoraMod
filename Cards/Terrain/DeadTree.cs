using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadTree = $"{GUID}_DeadTree";

	private void Add_Card_DeadTree()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Reach)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameDeadTree, "Dead Tree")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
