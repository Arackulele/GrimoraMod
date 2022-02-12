using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRevenant = "GrimoraMod_Revenant";

	private void Add_Revenant()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(3)
			.SetDescription("The Revenant, bringing the scythe of death.")
			.SetNames(NameRevenant, "Revenant")
			.Build()
		);
	}
}
