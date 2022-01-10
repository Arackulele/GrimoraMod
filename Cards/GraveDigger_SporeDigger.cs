using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameGraveDigger = "ara_Gravedigger";
		public const string NameSporeDigger = "ara_Sporedigger";

		private void AddAra_GraveDigger()
		{
			ApiUtils.Add(
				NameGraveDigger, "Gravedigger",
				"He spends his time alone digging for Bones in hopes of finding a treasure.",
				0, 3, 1, Resources.GraveDigger, Ability.BoneDigger);
		}

		private void AddAra_SporeDigger()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.BoneDigger,
				Ability.BoneDigger
			};

			List<Trait> traits = new List<Trait>()
			{
				Trait.Fused
			};

			ApiUtils.Add(NameSporeDigger, "Sporedigger", "The SporeDigger, an excellent digger.", 0,
				3, 1,
				Resources.SporeDigger,
				abilities,
				metaCategory: CardMetaCategory.Rare, appearanceBehaviour: CardUtils.getRareAppearance, traits: traits);
		}
	}
}