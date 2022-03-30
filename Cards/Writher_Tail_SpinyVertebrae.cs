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
			// .SetDescription("A SENSE OF DREAD CONSUMES YOU AS YOU REALIZE YOU ARE NOT ALONE IN THESE WOODS.")
			.SetNames(NameWritherTail, "Spiny Vertebrae")
			.Build();
	}
}
