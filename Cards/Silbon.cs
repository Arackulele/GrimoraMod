using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSilbon = "GrimoraMod_Silbon";

	private void Add_Silbon()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(InvertedStrike.ability, Ability.Strafe)
			.SetBaseAttackAndHealth(3, 2)
			.SetBoneCost(7)
			.SetDescription("A SKILLED HUNTER. WHAT DID IT HUNT? THAT IS UNKNOWN...")
			.SetNames(NameSilbon, "Silbon")
			.Build();
	}
}
