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
				NameGraveDigger, "Gravedigger", 0, 3,
				"He spends his time alone digging for Bones in hopes of finding a treasure.", 
				1, Resources.GraveDigger, Ability.BoneDigger
			);
		}
		
		private void AddAra_SporeDigger()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory> { CardMetaCategory.Rare };

			List<Ability> abilities = new List<Ability>
			{
				Ability.BoneDigger,
				Ability.BoneDigger
			};

			List<Trait> traits = new List<Trait>()
			{
				Trait.Fused
			};

			NewCard.Add(NameSporeDigger, "Sporedigger", 
				0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature,
				"The SporeDigger, an excellent digger.", bonesCost: 1, 
				appearanceBehaviour: CardUtils.getRareAppearance, traits: traits, 
				abilities: abilities, defaultTex: ImageUtils.LoadTextureFromResource(Resources.SporeDigger)
			);
		}
	}
}