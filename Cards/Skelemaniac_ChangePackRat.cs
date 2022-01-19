using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string DisplayNameSkelemaniac = "Skelemaniac";
	public const string NameSkelemaniac = "ara_" + DisplayNameSkelemaniac;

	private void AddAra_Skelemaniac()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.GuardDog)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(4)
			.SetDescription("A skeleton gone mad. At least it follows your command.")
			.SetNames(NameSkelemaniac, DisplayNameSkelemaniac)
			.Build()
		);
	}

	private void ChangePackRat()
	{
		List<Ability> abilities = new List<Ability> { Ability.GuardDog };

		new CustomCard("PackRat")
		{
			displayedName = DisplayNameSkelemaniac,
			baseAttack = 1,
			cost = 0,
			bonesCost = 4,
			baseHealth = 3,
			abilities = abilities,
			// tex = tex,
			description = "A skeleton gone mad. At least it follows your command."
		};
	}
}
