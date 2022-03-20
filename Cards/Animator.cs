using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameAnimator = $"{GUID}_Animator";

	private void Add_Animator()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Puppeteer.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(5)
			.SetDescription("The puppet becomes the puppeteer, yet still forever cursed by their dangling restraints.")
			.SetNames(NameAnimator, "Animator")
			.Build();
	}
}
