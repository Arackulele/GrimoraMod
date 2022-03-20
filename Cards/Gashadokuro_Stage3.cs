using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGashadokuro = $"{GUID}_Gashadokuro";

	private void Add_Gashadokuro_Stage3()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.TriStrike)
			.SetBaseAttackAndHealth(2, 3)
			// .SetDescription("THE CYCLE OF THE MUMMY LORD, NEVER ENDING.")
			.SetNames(NameGashadokuro, "Gashadokuro")
			.Build();
	}
}
