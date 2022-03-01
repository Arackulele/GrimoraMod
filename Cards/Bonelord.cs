namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = "GrimoraMod_Bonelord";

	private void Add_Bonelord()
	{
		CardBuilder.Builder
			.SetAbilities(BoneLordsReign.ability)
			.SetBaseAttackAndHealth(1, 20)
			.SetBoneCost(666)
			.SetDescription("WHEN THE BONE LORD APPEARS, EVERY CREATURE WILL FALL.")
			.SetNames(NameBonelord, "The Bone Lord")
			.Build();
	}
}
