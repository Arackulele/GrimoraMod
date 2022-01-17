using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NameFlames = "ara_Flames";

		private void AddAra_Flames()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.BuffNeighbours,
				Ability.Brittle
			};

			ApiUtils.Add(
				NameFlames, "Flames",
				"", 0, 0, 1, 2, Properties.Resources.Flames, abilities);
		}
	}
}