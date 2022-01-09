using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameSkeletonMage = "ara_SkeletonMage";
		
		private void AddAra_SkeletonMage()
		{
			
			List<Ability> abilities = new List<Ability> { Ability.Brittle };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.SkeletonMage);

			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy4);

			List<Texture> decals = new() { decalTex };
			
			NewCard.Add(
				NameSkeletonMage, "Skelemagus", 4, 1,
				CardUtils.getNormalCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				"The Skelemagus, they have learned the ancient spell of Death.",
				abilities: abilities, defaultTex: tex, decals: decals
			);
		}
	}
}