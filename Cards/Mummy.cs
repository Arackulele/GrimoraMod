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
				NameMummy, "Mummy Lord",
				"The cycle of the Mummy Lord is never ending.", 3, 3,
				2, Resources.Mummy, new List<Ability>(), CardMetaCategory.GBCPlayable);
		}
	}
}