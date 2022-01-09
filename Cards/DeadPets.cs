using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameDeadPets = "ara_DeadPets";
		
		private void AddAra_DeadPets()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability>
			{
				Ability.TripleBlood,
				Ability.Sacrificial
			};

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.DeadPets);

			NewCard.Add(NameDeadPets, "Pharaoh's Pets", 3, 1,
				metaCategories, CardComplexity.Intermediate, CardTemple.Nature,
				"The undying pets of the Pharaoh.", bonesCost: 4,
				appearanceBehaviour: CardUtils.getRareAppearance, abilities: abilities, defaultTex: tex
			);
		}
	}
}