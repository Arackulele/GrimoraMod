using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameTombRobber = "ara_TombRobber";
		
		private void AddAra_TombRobber()
		{
			ApiUtils.Add(
				NameTombRobber, "Tomb Robber", 0, 1,
				"Nothing... Nothing again... No treasure is left anymore.", 0,
				Resources.TombRobber, Ability.ActivatedDrawSkeleton,  complexity: CardComplexity.Advanced
			);
		}
	}
}