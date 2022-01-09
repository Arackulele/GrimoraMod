using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameMudWorm = "ara_MudWorm";
		
		private void AddAra_RingWorm()
		{
			ApiUtils.Add(NameMudWorm, "Mud Worm", 2, 1,
				"Like an true worm, loves to dig in the dirt.", 5,
				Resources.RingWorm, Ability.BoneDigger
			);
		}
	}
}