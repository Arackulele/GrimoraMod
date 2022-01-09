using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{

		public const string DisplayNameSkelemaniac = "Skelemaniac";


		private void AddAra_Skelemaniac()
		{
			ApiUtils.Add(DisplayNameSkelemaniac, DisplayNameSkelemaniac, 1, 3,
				"A skeleton gone mad. At least it follows your command.", 
				4, Resources.Skelemaniac, Ability.GuardDog
			);
		}
		
		private void ChangePackRat()
		{
			List<Ability> abilities = new List<Ability> { Ability.GuardDog };

			Texture2D tex = ImageUtils.LoadTextureFromResource(Resources.Skelemaniac);

			new CustomCard("PackRat")
			{
				displayedName = DisplayNameSkelemaniac,
				baseAttack = 1,
				cost = 0,
				bonesCost = 4,
				baseHealth = 3,
				abilities = abilities,
				tex = tex,
				description = "A skeleton gone mad. At least it follows your command."
			};
		}
	}
}