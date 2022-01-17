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
				Ability.DrawCopyOnDeath,
				Ability.Brittle
			};

			ApiUtils.Add(NameDeadPets, "Pharaoh's Pets", "The undying underlings of the Pharaoh.", 0,
				3, 1, 4, Resources.DeadPets,
				complexity: CardComplexity.Intermediate,
				metaCategory: CardMetaCategory.Rare,
				appearanceBehaviour: CardUtils.getRareAppearance, abilities: abilities
			);
		}
	}
}