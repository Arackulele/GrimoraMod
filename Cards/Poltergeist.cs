using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePoltergeist = $"{GUID}_Poltergeist";

	private void Add_Card_Poltergeist()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("AN EVIL TRICKSTER, THE POLTERGEIST HAS ENJOYED THEIR DEATH VERY MUCH.")
			.SetNames(NamePoltergeist, "Poltergeist")
			.Build();
	}
}
