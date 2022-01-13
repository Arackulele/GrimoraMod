using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameSkeletonArmy = "ara_SkeletonArmy";
		
		private void AddAra_SkeletonArmy()
		{
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy2);

			List<Texture> decals = new() { decalTex };
			
			ApiUtils.Add(
				NameSkeletonArmy, "Skeleton Army",
				"The skeleton army, boons of the Bone Lord.", 2, 
				4,
				2, Resources.SkeletonArmy, Ability.SkeletonStrafe, decals: decals);

			// new CustomCard("Goat")
			// {
			// 	displayedName = NameSkeletonArmy,
			// 	baseAttack = 2,
			// 	baseHealth = 4,
			// 	energyCost = 2,
			// 	cost = 0,
			// 	tex = defaultTex,
			// 	abilities = abilities,
			// 	decals = decals,
			// 	metaCategories = metaCategories,
			// 	description = "The skeleton army, boons of the Bone Lord."
			// };
		}
	}
}