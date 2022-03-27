using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = $"{GUID}_Bonelord";

	private void Add_Card_Bonelord()
	{
		CardBuilder.Builder
			.SetAbilities(BonelordsReign.ability, GiantStrike.ability, Ability.Reach, Ability.MadeOfStone)
			.SetSpecialAbilities(GrimoraGiant.FullSpecial.Id)
			.SetBaseAttackAndHealth(1, 20)
			.SetBoneCost(666)
			.SetDescription("WHEN THE BONELORD APPEARS, EVERY CREATURE WILL FALL.")
			.SetTraits(Trait.Giant, Trait.Uncuttable)
			.SetNames(NameBonelord, "The Bonelord")
			.Build();
	}
}
