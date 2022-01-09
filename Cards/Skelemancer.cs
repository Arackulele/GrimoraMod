using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameSkelemancer = "ara_Skelemancer";
		
		private void AddAra_Skelemancer()
		{
			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Resources.SkeletonJuniorSage);

			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy2);

			List<Texture> decals = new() { decalTex };

			NewCard.Add(
				NameSkelemancer, "Skelemancer", 1, 1,
				CardUtils.getNormalCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				"The humble Skelemancer, he likes a good fight.", energyCost: 2,
				defaultTex: defaultTex, decals: decals
			);
		}
	}
}