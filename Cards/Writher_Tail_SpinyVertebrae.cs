using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWritherTail = $"{GUID}_Writher_tail";

	private void Add_Card_WritherTail()
	{
		CardBuilder.Builder
			.SetAppearance(CardAppearanceBehaviour.Appearance.RareCardBackground)
			.SetAbilities(Ability.Sharp)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameWritherTail, "Spiny Vertebrae")
			.Build();
	}
}
