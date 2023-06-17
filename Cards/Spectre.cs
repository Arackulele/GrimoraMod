using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSpectre = $"{GUID}_Spectre";

	private void Add_Card_Spectre()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(HauntingCall.ability)
			.SetBaseAttackAndHealth(3, 3)
			.SetEnergyCost(6)
			.SetDescription("THE SPECTRE EMITS A FLUTE LIKE CALL, AN OMEN OF BOTH DEATH AND DESCTRUCTION")
			.SetNames(NameSpectre, "Spectre")
			.Build();
	}
}
