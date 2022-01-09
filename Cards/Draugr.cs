using System.Collections.Generic;
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
			List<Ability> abilities = new List<Ability> { Ability.IceCube };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Draugr);

			NewCard.Add(NameDraugr, "Draugr", 0, 1, 
				CardUtils.getNormalCardMetadata, CardComplexity.Intermediate, CardTemple.Nature,
				"Hiding in a suit of armor, this skeleton won't last forever.", 
				bonesCost: 1, abilities: abilities, defaultTex: tex, 
				iceCubeId: new IceCubeIdentifier("Squirrel")
			);
		}
	}
}