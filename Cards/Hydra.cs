using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameHydra = "ara_Hydra";
		
		private void AddAra_Hydra()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability>
			{
				Ability.DrawCopyOnDeath,
				Ability.TriStrike
			};

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Hydra);

			NewCard.Add(NameHydra, "Hydra", 1, 1,
				metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				"Described by some as the truest nightmare", bonesCost: 4,
				abilities: abilities, defaultTex: tex,
				appearanceBehaviour: CardUtils.getRareAppearance
			);
		}
	}
}