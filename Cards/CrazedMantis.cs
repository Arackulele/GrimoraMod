using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameCrazedMantis = "ara_CrazedMantis";
		private void AddAra_CrazedMantis()
		{
			ApiUtils.Add(
				NameCrazedMantis, "Crazed Mantis",
				"The poor mantis has gone insane.", 1,
				1, 4, Resources.Mantis, Ability.SplitStrike);
		}
	}
}