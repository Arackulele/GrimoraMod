using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFrostGiant = $"{GUID}_FrostGiant";

	private void Add_Card_Glacier_Stage2_FrostGiant()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(2, 4)
			.SetBoneCost(10)
			.SetNames(NameFrostGiant, "Frost Giant")
			.SetTraits(Trait.Giant, Trait.Uncuttable)
			.Build()
			;
	}
}
