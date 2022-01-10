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
			Texture2D decalTex = ImageUtils.LoadTextureFromResource(Resources.Energy3);

			List<Texture> decals = new() { decalTex };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Obol);

			ApiUtils.Add(NameObol, "Ancient Obol",
				"The Ancient Obol, the Bone Lord likes this one.", 0,
				6, 0, Resources.Obol, Ability.Sharp, decals: decals);
		}
	}
}