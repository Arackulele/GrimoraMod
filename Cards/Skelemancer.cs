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
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy2);

			List<Texture> decals = new() { decalTex };

			ApiUtils.Add(
				NameSkelemancer, "Skelemancer",
				"The humble Skelemancer, he likes a good fight.",  1, 
				1, 2, Resources.SkeletonJuniorSage, decals: decals);
		}
	}
}