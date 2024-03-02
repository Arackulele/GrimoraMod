namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCatacomb = $"{GUID}_Catacomb";

	private void Add_Card_Catacomb()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetSpecialAbilities(GainAttackBones.FullSpecial.Id)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(10)
			.SetIceCube(NameBonepile)
			.SetDescription("A GROUP OF SKELETONS IS CALLED A CATACOMB. THIS IS A RATHER LARGE GATHERING.")
			.SetNames(NameCatacomb, "Catacomb")
			.SetSpecialStatIcon(GainAttackBones.FullStatIcon.Id)
			.Build();
	}
}
