using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameMummy = "ara_Mummy";
		
		private void AddAra_Mummy()
		{
			ApiUtils.Add(
				NameMummy, "Mummy Lord", 3, 3,
				"The cycle of the Mummy Lord is never ending.", 2, Resources.Mummy,
				new List<Ability>(), CardMetaCategory.GBCPlayable
			);
		}
	}
}