using System.Collections.Generic;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameUndeadWolf = "ara_UndeadWolf";
		
		private void AddAra_Wolf()
		{
			ApiUtils.Add(NameUndeadWolf, "Undead Wolf",
				"A diseased wolf. The pack has left him for death.", 4, 3, 2,
				4, Resources.Wolf, new List<Ability>(), CardMetaCategory.ChoiceNode);
		}
	}
}