using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameObol = "ara_Obol";
		
		private void AddAra_Obol()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>
			{
				CardMetaCategory.ChoiceNode,
				CardMetaCategory.TraderOffer
			};

			List<Ability> abilities = new List<Ability> { Ability.Sharp };
			
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy3);

			List<Texture> decals = new() { decalTex };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Obol);

			NewCard.Add(NameObol, "Ancient Obol", 0, 6, 
				metaCategories, CardComplexity.Intermediate, CardTemple.Nature,
				"The Ancient Obol, the Bone Lord likes this one.", energyCost: 3, abilities: abilities,
				defaultTex: tex, decals: decals
			);
		}
	}
}