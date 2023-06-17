namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCatacomb = $"{GUID}_Catacomb";

	private void Add_Card_Catacomb()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetSpecialAbilities(LammergeierAttack.FullSpecial.Id)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(10)
			.SetDescription("A GROUP OF SKELETONS IS CALLED A CATACOMB. THIS IS A RATHER LARGE GATHERING.")
			.SetNames(NameCatacomb, "Catacomb")
			.SetSpecialStatIcon(LammergeierAttack.FullStatIcon.Id)
			.Build();
	}
}
