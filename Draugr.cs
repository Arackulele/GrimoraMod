using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameDraugr = "ara_Draugr";

		private void AddAra_Draugr()
		{
			ApiUtils.Add(NameDraugr, "Draugr",
				"Hiding in a suit of armor, this skeleton won't last forever.", 0, 0,
				1, 1,
				Resources.Draugr,
				Ability.IceCube, CardMetaCategory.ChoiceNode, iceCubeId: new IceCubeIdentifier("Skeleton"));
		}
	}
}