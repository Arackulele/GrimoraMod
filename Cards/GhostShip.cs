using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameGhostShip = "ara_GhostShip";
		
		private void AddAra_GhostShip()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.SkeletonStrafe,
				Ability.Submerge
			};
			
			ApiUtils.Add(NameGhostShip, "Ghost Ship",
				"The skeleton army never rests.", 
				0, 1, 4, Resources.GhostShip, abilities, complexity: CardComplexity.Advanced);
		}
	}
}