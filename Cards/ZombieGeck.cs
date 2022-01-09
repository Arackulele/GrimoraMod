using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameZombieGeck = "ara_ZombieGeck";
		
		private void AddAra_ZombieGeck()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability> { Ability.Brittle };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Properties.Resources.Geck);

			NewCard.Add(
				NameZombieGeck, "Geckz", 
				2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature,
				"A bit famished. Could use a bite to eat.", abilities: abilities, defaultTex: tex,
				appearanceBehaviour: CardUtils.getRareAppearance
			);
		}
	}
}