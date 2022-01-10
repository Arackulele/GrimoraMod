using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameBoneSnapper = "ara_BoneSnapper";
		
		private void AddAra_Snapper()
		{
			ApiUtils.Add(NameBoneSnapper, "Bone Snapper",
				"Snap snap your Bones are gone.", 1, 6,
				5, Resources.Snapper, new List<Ability>());
		}
	}
}