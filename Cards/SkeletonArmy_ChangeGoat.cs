using System.Collections.Generic;
using APIPlugin;
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
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

			List<Ability> abilities = new List<Ability> { Ability.SkeletonStrafe };

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Resources.SkeletonArmy);

			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy2);

			List<Texture> decals = new() { decalTex };
			
			NewCard.Add(
				NameSkeletonArmy, "Skeleton Army", 2, 4,
				metaCategories, CardComplexity.Vanilla, CardTemple.Nature, 
				"The skeleton army, boons of the Bone Lord.", energyCost: 2, bloodCost: 0,
				abilities: abilities, decals: decals, defaultTex: defaultTex
			);

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