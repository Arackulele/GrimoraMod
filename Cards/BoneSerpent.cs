using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		
		public const string NameBoneSerpent = "ara_BoneSerpent";

		private void AddAra_BoneSerpent()
		{
			ApiUtils.Add(NameBoneSerpent, "Bone Serpent", 1, 1,
				"The poison strike will melt even the most dense bones.", 4,
				Resources.Adder, Ability.Deathtouch
			);
		}
	}
}