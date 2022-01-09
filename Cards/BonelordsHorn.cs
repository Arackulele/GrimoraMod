using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameBoneLordsHorn = "ara_BonelordsHorn";
		
		private void AddAra_BonelordsHorn()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability>
			{
				Ability.QuadrupleBones,
				Ability.IceCube
			};

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Resources.BoneLordsHorn);
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy4);

			List<Texture> decals = new() { decalTex };
			
			NewCard.Add(NameBoneLordsHorn, "Bone Lord's Horn",
				0, 1, metaCategories, CardComplexity.Advanced,
				CardTemple.Nature, "The Horn of the Bonelord, you do not want to find out what's inside.",
				energyCost: 4, appearanceBehaviour: CardUtils.getRareAppearance, abilities: abilities,
				defaultTex: defaultTex, decals: decals, iceCubeId: new IceCubeIdentifier(NameBonePrince));
		}
	}
}