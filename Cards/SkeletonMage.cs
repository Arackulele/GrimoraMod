using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameSkeletonMage = "ara_SkeletonMage";

		private void AddAra_SkeletonMage()
		{
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy4);

			List<Texture> decals = new() { decalTex };

			ApiUtils.Add(
				NameSkeletonMage, "Skelemagus",
				"The Skelemagus, they have learned the ancient spell of Death.",
				4, 1, 1, Resources.SkeletonMage, Ability.Brittle, decals: decals);
		}
	}
}