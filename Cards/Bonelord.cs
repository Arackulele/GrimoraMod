using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = "GrimoraMod_Bonelord";

	private void Add_Bonelord()
	{
		CardBuilder.Builder
			.SetAbilities(BoneLordsReign.ability, GiantStrike.ability, Ability.Reach)
			.SetAbilities(GrimoraGiant.SpecialTriggeredAbility)
			.SetBaseAttackAndHealth(1, 20)
			.SetBoneCost(666)
			.SetDescription("WHEN THE BONE LORD APPEARS, EVERY CREATURE WILL FALL.")
			.SetTraits(Trait.Giant, Trait.Uncuttable)
			.SetNames(NameBonelord, "The Bone Lord")
			.Build();
	}
}
