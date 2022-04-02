using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGashadokuro = $"{GUID}_Gashadokuro";

	private void Add_Card_Gashadokuro_Stage3()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.TriStrike)
			.SetBaseAttackAndHealth(2, 3)
			.SetNames(NameGashadokuro, "Gashadokuro")
			.Build();
	}
}
