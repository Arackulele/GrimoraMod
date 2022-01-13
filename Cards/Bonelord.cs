using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameBonelord = "ara_Bonelord";

		private void AddAra_Bonelord()
		{
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy6);

			List<Texture> decals = new() { decalTex };

			ApiUtils.Add(NameBonelord, "The Bone Lord", "Lord of Bones, Lord of Bones, answer our call.",
				5, 10, 6, Resources.BoneLord,
				appearanceBehaviour: CardUtils.getRareAppearance,
				ability: Ability.Deathtouch,
				metaCategory: CardMetaCategory.Rare,
				decals: decals
			);
		}
	}
}