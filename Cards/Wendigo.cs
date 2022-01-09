using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		private void AddAra_Wendigo()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability>
			{
				Ability.Strafe,
				Ability.DebuffEnemy
			};

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Resources.Wendigo);

			NewCard.Add("ara_Wendigo", "Wendigo", 2, 2, 
				metaCategories, CardComplexity.Vanilla, CardTemple.Nature,
				"Described by some as the truest nightmare", bonesCost: 5, 
				abilities: abilities, defaultTex: defaultTex, appearanceBehaviour: CardUtils.getRareAppearance
			);
		}
	}
}