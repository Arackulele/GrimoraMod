using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameEmberSpirit = "ara_Ember_Spirit";

		private void AddAra_Ember_spirit()
		{
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy2);

			List<Texture> decals = new() { decalTex };

			ApiUtils.Add(NameEmberSpirit, "Spirit of Ember",
				"A trickster spirit fleeing and leaving behind its flames.",
				2, 1, 3, 2, Resources.Ember,
				metaCategory: CardMetaCategory.Rare,
				appearanceBehaviour: CardUtils.getRareAppearance, 
				decals: decals, 
				ability: FlameStrafe.ability
			);
		}
	}
}