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
			ApiUtils.Add(NameBoneSnapper, "Bone Snapper", 1, 6,
				"Snap snap your Bones are gone.", 5, Resources.Snapper,
				new List<Ability>()
			);
		}
	}
}