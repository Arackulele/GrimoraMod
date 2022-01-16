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

			List<Ability> abilities = new List<Ability>
			{
			};

			ApiUtils.Add(NameBoneSnapper, "Bone Snapper",
				"One bite of this Vile being is strong enough to break even Bones.",0, 1, 6,
				5, Resources.Snapper, abilities, CardMetaCategory.ChoiceNode);

        }
    }
}