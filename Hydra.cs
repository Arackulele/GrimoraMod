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
			List<Ability> abilities = new List<Ability>
			{
				Ability.DrawCopyOnDeath,
				Ability.TriStrike
			};

			ApiUtils.Add(NameHydra, "Hydra",
				"Described by some as the truest nightmare", 0, 1, 
				1, 4,
				Resources.Hydra, abilities,
				CardMetaCategory.Rare, CardComplexity.Advanced, appearanceBehaviour: CardUtils.getRareAppearance);
		}
	}
}