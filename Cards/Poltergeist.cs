using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePoltergeist = $"{GUID}_Poltergeist";

	private void Add_Card_Poltergeist()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetSpecialStatIcon(GainAttackTerrain.FullStatIcon.Id)
			.SetAbilities(Ability.Submerge)
			.SetBaseAttackAndHealth(0, 1)
			.SetEnergyCost(4)
			.SetSpecialAbilities(GainAttackTerrain.FullSpecial.Id)
			.SetDescription("AN EVIL TRICKSTER, THE POLTERGEIST HAS ENJOYED THEIR DEATH VERY MUCH.")
			.SetNames(NamePoltergeist, "Poltergeist")
			.Build();
	}
}
