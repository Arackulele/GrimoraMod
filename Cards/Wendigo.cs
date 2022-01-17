using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWendigo = "ara_Wendigo";

	private void AddAra_Wendigo()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.DebuffEnemy,
			Ability.Strafe
		};

		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(2, 2)
			.WithBonesCost(5)
			.WithDescription("A sense of dread consumes you as you realize you are not alone in these woods.")
			.WithNames(NameWendigo, "Wendigo")
			.WithPortrait(Resources.Wendigo)
			.Build()
		);
	}
}