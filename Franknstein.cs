using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameFranknstein = "ara_Franknstein";

		private void AddAra_Franknstein()
		{
			List<Ability> abilities = new List<Ability>
			{
			};

			ApiUtils.Add(NameFranknstein, "Frank & Stein",
				"Best friends, brothers, and fighters.", 0, 2,
				2, 5, Resources.Franknstein, abilities, CardMetaCategory.ChoiceNode);
		}
	}
}