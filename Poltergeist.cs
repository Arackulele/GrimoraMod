using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		public const string NamePoltergeist = "ara_Poltergeist";

		private void AddAra_Poltergeist()
		{
			List<Ability> abilities = new List<Ability>
			{
				Ability.Submerge,
				Ability.Flying
			};

			ApiUtils.Add(NamePoltergeist, "Poltergeist", 
				"A skilled haunting ghost. Handle with caution.", 2,
				1, 1, 0, 
				Resources.Poltergeist, abilities, CardMetaCategory.ChoiceNode
			);
		}
	}
}