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
			ApiUtils.Add(NameZombie, "Zombie", 1, 1,
				"The humble zombie, a respected member of the army.", 2, 
				Properties.Resources.Zombie
			);
		}
	}
}