using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadPets = "ara_DeadPets";

	private void AddAra_DeadPets()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.DrawCopyOnDeath,
			Ability.Brittle
		};

		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(3, 1)
			.WithBonesCost(4)
			.WithDescription("The undying underlings of the Pharaoh.")
			.WithNames(NameDeadPets, "Pharaoh's Pets")
			.WithPortrait(Resources.DeadPets)
			.Build()
		);
	}
}