using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		
		public const string NameBonepile = "ara_Bonepile";
		
		private void AddAra_Bonepile()
		{


			ApiUtils.Add(NameBonepile, "Bone Heap",
				"An uninspiring pile of bones. You can have it.",
				0, 0, 1, 1, Resources.BonePile, Ability.QuadrupleBones, metaCategory: CardMetaCategory.ChoiceNode);
		}
	}
}