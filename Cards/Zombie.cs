namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombie = $"{GUID}_Zombie";

	private void Add_Zombie()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(2)
			.SetDescription("THE HUMBLE ZOMBIE, A RESPECTED MEMBER OF THE ARMY.")
			.SetNames(NameZombie, "Zombie")
			.Build();
	}
}
