using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string NameZombie = "ara_Zombie";
		
		private void AddAra_Zombie()
		{

			List<Ability> abilities = new List<Ability>
			{
			};

			ApiUtils.Add(NameZombie, "Zombie",
				"The humble zombie, a respected member of the army.", 0, 1,
				1, 2, Properties.Resources.Zombie, abilities, CardMetaCategory.ChoiceNode);
		}
	}
}