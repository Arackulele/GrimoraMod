using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		private void AddAra_Wendigo()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.Strafe,
				Ability.DebuffEnemy
			};

			Texture2D defaultTex = ImageUtils.LoadTextureFromResource(Resources.Wendigo);

			ApiUtils.Add("ara_Wendigo", "Wendigo",
				"Described by some as the truest nightmare", 0, 2,
				2, 5,
				Resources.Wendigo, abilities, CardMetaCategory.Rare );
		}
	}
}