using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameDeadPets = "ara_DeadPets";

		private void AddAra_DeadPets()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.TripleBlood,
				Ability.Sacrificial
			};

			ApiUtils.Add(NameDeadPets, "Pharaoh's Pets", "The undying pets of the Pharaoh.",
				3, 1, 4, Resources.DeadPets,
				complexity: CardComplexity.Intermediate,
				metaCategory: CardMetaCategory.Rare,
				appearanceBehaviour: CardUtils.getRareAppearance, abilities: abilities
			);
		}
	}
}