using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
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
				tex = ImageUtils.LoadTextureFromResource(Resources.Skeleton)
			};
		}
	}
}