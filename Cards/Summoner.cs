using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSummoner = "GrimoraMod_Summoner";

	private void AddAra_Summoner()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("This Skeleton learned Necromancy just to not fall in one Strike.")
			.SetNames(NameSummoner, "Summoner")
			.Build()
		);
	}
}
