using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameZombieGeck = "ara_ZombieGeck";

		private void AddAra_ZombieGeck()
		{
			List<Ability> abilities = new List<Ability> { Ability.Brittle };

			ApiUtils.Add(
				NameZombieGeck, "Zomb-Geck", "A bit famished. Could use a bite to eat.", 0, 2,
				1, 1, Resources.Geck,
				abilities, CardMetaCategory.Rare, appearanceBehaviour: CardUtils.getRareAppearance);
		}
	}
}