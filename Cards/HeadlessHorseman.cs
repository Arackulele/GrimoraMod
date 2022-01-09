using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameHeadlessHorseman = "ara_HeadlessHorseman";
		
		private void AddAra_HeadlessHorseman()
		{
			
			List<Ability> abilities = new List<Ability>
			{
				Ability.Strafe,
				Ability.Flying
			};

			ApiUtils.Add(
				NameHeadlessHorseman, "Headless Horseman", 4, 3,
				"The apocalypse is soon.", 9, Resources.HeadlessHorseman, abilities
			);
		}
	}
}