using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCandyMonster = $"{GUID}_CandyMonster";

	private void Add_Card_CandyMonster()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.WhackAMole, Ability.ExplodeOnDeath, Collector.ability)
			.SetSpecialAbilities(GainAttackBones.FullSpecial.Id)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(5)
			.SetDescription("HAPPY HALLOWEEN!")
			.SetNames(NameCandyMonster, "Candy Monster")
			.SetTraits(Trait.Uncuttable, Trait.Giant)
			.SetSpecialStatIcon(GainAttackCandy.FullStatIcon.Id)
			.Build();
	}
}
