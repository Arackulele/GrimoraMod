using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameBoneLordsHorn = "ara_BonelordsHorn";

		private void AddAra_BonelordsHorn()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.QuadrupleBones,
				Ability.IceCube
			};


			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy4);

			List<Texture> decals = new() { decalTex };

			ApiUtils.Add(NameBoneLordsHorn, "Bone Lord's Horn", "The Horn of the Bonelord, you do not want to find out what's inside.",4,
				0, 1, 0,
				Resources.BoneLordsHorn, abilities: abilities,
				metaCategory: CardMetaCategory.Rare, decals: decals, appearanceBehaviour: CardUtils.getRareAppearance, iceCubeId: new IceCubeIdentifier(NameBonePrince));
		}
	}
}