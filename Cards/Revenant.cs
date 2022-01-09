using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameRevenant = "ara_Revenant";
		
		private void AddAra_Revenant()
		{
			ApiUtils.Add(NameRevenant, "Revenant", 3, 1,
				"The Revenant, bringing the scythe of death.", 3,
				Resources.Revenant, Ability.Brittle,  complexity: CardComplexity.Intermediate
			);
		}
	}
}