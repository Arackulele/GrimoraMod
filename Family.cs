using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameFamily = "ara_Family";
		
		private void AddAra_Family()
		{
			ApiUtils.Add(NameFamily, "The Walkers",
				"The family wishes to rest in piece.", 0, 1, 2,
				4, Resources.Walkers, Ability.QuadrupleBones, metaCategory: CardMetaCategory.ChoiceNode);

		}
	}
}