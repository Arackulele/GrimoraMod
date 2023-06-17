using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = $"{GUID}_Bonelord";

	private void Add_Card_Bonelord()
	{
		CardBuilder.Builder
			.SetAbilities(GiantStrike.ability, Ability.Reach, Ability.MadeOfStone)
			.SetSpecialAbilities(GrimoraGiant.FullSpecial.Id)
			.SetBaseAttackAndHealth(1, 20)
			.SetBoneCost(666)
			.SetDescription("SO YOU HAVE MADE IT HERE, YOU CANNOT END WHAT I HAVE STARTED, IT HAS GONE TOO FAR.")
			.SetTraits(Trait.Giant, Trait.Uncuttable)
			.SetNames(NameBonelord, "The Bonelord")
			.Build();
	}
}
