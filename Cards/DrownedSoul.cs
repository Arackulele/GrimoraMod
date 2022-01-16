using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameDrownedSoul = "ara_DrownedSoul";
		
		private void AddAra_DrownedSoul()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.Deathtouch,
				Ability.Submerge
			};

			ApiUtils.Add(NameDrownedSoul, "Drowned Soul",
				"Going into that well wasn't the best idea...", 0, 1,
				1, 4, Resources.DrownedSoul, abilities, CardMetaCategory.ChoiceNode);
		}
	}
}