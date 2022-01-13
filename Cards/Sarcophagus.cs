using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameSarcophagus = "ara_Sarcophagus";

		private void AddAra_Sarcophagus()
		{
			ApiUtils.Add(NameSarcophagus, "Sarcophagus",
				"The cycle of the Mummy Lord, never ending.", 0,
				2, 4,
				Resources.Sarcophagus,
				Ability.Evolve, complexity: CardComplexity.Intermediate, evolveId: new EvolveIdentifier("ara_Mummy", 1));
		}
	}
}