using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameNecromancer = "ara_Necromancer";
		
		private void AddAra_Necromancer()
		{
			ApiUtils.Add(
				NameNecromancer, "Necromancer",
				"The vicious Necromancer, nothing dies once.",
				0, 1, 2, 3, Resources.Necromancer, Ability.DoubleDeath, CardMetaCategory.Rare, CardComplexity.Advanced);
		}
	}
}