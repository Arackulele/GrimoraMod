using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameEmberSpirit = "ara_Ember_Spirit";
		
		private void AddAra_Ember_spirit()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability> { FlameStrafe.ability };

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Properties.Resources.Ember);

			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Properties.Resources.Energy2);

			List<Texture> decals = new() { decalTex };

			NewCard.Add(NameEmberSpirit, "Spirit of Ember", 1, 3, 
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				"A trickster spirit fleeing and leaving behind its flames.", bonesCost: 2, energyCost: 3,
				appearanceBehaviour: CardUtils.getRareAppearance, defaultTex: defaultTex, decals: decals, abilities: abilities
			);
		}
	}
}