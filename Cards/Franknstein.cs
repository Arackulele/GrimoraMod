using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameFranknstein = "ara_Franknstein";

		private void AddAra_Franknstein()
		{
			ApiUtils.Add(NameFranknstein, "Frank & Stein", 2, 2,
				"Best friends, brothers, and fighters.", 5,
				Resources.Franknstein
			);
		}
	}
}