using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSummoner = "GrimoraMod_Summoner";

	private void Add_Summoner()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("THIS SKELETON LEARNED NECROMANCY JUST TO NOT FALL IN ONE STRIKE.")
			.SetNames(NameSummoner, "Summoner")
			.Build()
		);
	}
}
