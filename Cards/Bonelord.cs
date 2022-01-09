using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameBonelord = "ara_Bonelord";
		
		private void AddAra_Bonelord()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability> { Ability.Deathtouch };

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Properties.Resources.BoneLord);
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Properties.Resources.Energy6);

			List<Texture> decals = new() { decalTex };

			NewCard.Add(
				NameBonelord,
				"The Bone Lord",
				5, 10, metaCategories,
				CardComplexity.Simple, CardTemple.Nature,
				"Lord of Bones, Lord of Bones, answer our call.", bonesCost: 6, energyCost: 6,
				appearanceBehaviour: CardUtils.getRareAppearance, abilities: abilities,
				defaultTex: defaultTex, decals: decals);
		}
	}
}