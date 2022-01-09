using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		
		public const string NameBonepile = "ara_Bonepile";
		
		private void AddAra_Bonepile()
		{
			ApiUtils.Add(NameBonepile, "Bone Heap", 0, 1,
				"An uninspiring pile of bones. You can have it.",
				1, Resources.BonePile, Ability.QuadrupleBones
			);
		}
	}
}