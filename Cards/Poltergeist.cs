using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NamePoltergeist = "ara_Poltergeist";
		
		private void AddAra_Poltergeist()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>
			{
				CardMetaCategory.ChoiceNode,
				CardMetaCategory.TraderOffer
			};

			List<Ability> abilities = new List<Ability>
			{
				Ability.Submerge,
				Ability.Flying
			};

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Poltergeist);

			NewCard.Add(NamePoltergeist, "Poltergeist", 1, 1,
				metaCategories, CardComplexity.Vanilla, CardTemple.Nature,
				"A skilled haunting ghost. Handle with caution.", 
				energyCost: 3, abilities: abilities, defaultTex: tex
			);
		}
	}
}