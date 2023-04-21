using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGravebard = $"{GUID}_Gravebard";

	private void Add_Card_Gravebard()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BuffNeighbours)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("THE LOWLY GRAVEBARD, MUSIC ALWAYS BROUGHT HIM COMFORT IN THE WORST OF TIMES. NOW, EVEN AT THE END OF THE WORLD HE SHARES HIS SONG.")
			.SetNames(NameGravebard, "Gravebard")
			.Build();
	}
}
