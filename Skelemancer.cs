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


			List<Ability> abilities = new List<Ability>
			{
			};

			ApiUtils.Add(
				NameSkelemancer, "Skelemancer",
				"The humble Skelemancer, they like a good fight.",  2,1, 
				1, 0, Resources.SkeletonJuniorSage, abilities: abilities,CardMetaCategory.ChoiceNode, decals: decals);
		}
	}
}