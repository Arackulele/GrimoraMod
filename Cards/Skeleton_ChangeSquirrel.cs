using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeleton = "Skeleton";

	private void ChangeSquirrel()
	{
		List<Ability> abilities = new List<Ability> { Ability.Brittle };

		new CustomCard("Squirrel")
		{
			displayedName = NameSkeleton,
			baseAttack = 1,
			abilities = abilities,
			// tex = ImageUtils.LoadTextureFromBytes(Resources.Skeleton)
		};
	}
}