using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSummoner = $"{GUID}_Summoner";

	private void Add_Summoner()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("THIS SKELETON LEARNED NECROMANCY JUST TO NOT FALL IN ONE STRIKE.")
			.SetNames(NameSummoner, "Summoner")
			.Build();
	}
}
