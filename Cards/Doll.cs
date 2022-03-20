namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDoll = $"{GUID}_Doll";

	private void Add_Doll()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Imbued.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(3)
			.SetDescription("A lonesome doll, returned from seas of slate and silent shores... it stares lifelessly.")
			.SetNames(NameDoll, "Doll")
			.Build();
	}
}
