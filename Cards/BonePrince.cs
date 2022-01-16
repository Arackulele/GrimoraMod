using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameBonePrince = "ara_BonePrince";

		private void AddAra_BonePrince()
		{
			ApiUtils.Add(NameBonePrince, "Bone Prince", "", 0, 2,
				1, 1, Resources.BonePrince, new List<Ability>(), CardMetaCategory.GBCPlayable);
		}
	}
}