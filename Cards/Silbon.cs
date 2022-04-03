using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSilbon = $"{GUID}_Silbon";

	private void Add_Card_Silbon()
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
