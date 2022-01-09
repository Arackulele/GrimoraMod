using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameSarcophagus = "ara_Sarcophagus";
		
		private void AddAra_Sarcophagus()
		{
			List<Ability> abilities = new List<Ability> { Ability.Evolve };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Sarcophagus);

			NewCard.Add(NameSarcophagus, "Sarcophagus", 0, 2,
				CardUtils.getNormalCardMetadata, CardComplexity.Intermediate, CardTemple.Nature,
				"The cycle of the Mummy Lord, never ending.", bonesCost: 4,
				abilities: abilities, defaultTex: tex,
				evolveId: new EvolveIdentifier("ara_Mummy", 1)
			);
		}
	}
}