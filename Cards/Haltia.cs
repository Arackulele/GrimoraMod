using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHaltia = $"{GUID}_Haltia";

	private void Add_Card_Haltia()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.LatchDeathShield, Ability.Flying)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(5)
			.SetDescription("A devoted guardian spirit. Even when dispelled, it grants a protective blessing.")
			.SetNames(NameHaltia, "Haltia")
			.Build();
	}
}
