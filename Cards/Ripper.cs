using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRipper = $"{GUID}_Ripper";

	private void Add_Card_Ripper()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(6, 6)
			.SetBoneCost(6)
			.SetNames(NameRipper, "Ripper")
			.SetDescription("THE RIPPER SOLVES EVERYTHING THROUGH SHEER STRENGTH, IT WILL PUNCH ANY ISSUE STRAIGHT IN THE FACE, AND SOLVE IT, TOO.")
			.Build();
	}
}
