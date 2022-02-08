using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWyvern = "GrimoraMod_Wyvern";

	private void Add_Wyvern()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(PayEnergyForWyvern.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("A Skeletal Beast, it calls in more of its kind.")
			.SetNames(NameWyvern, "Wyvern")
			.Build()
		);
	}
}
