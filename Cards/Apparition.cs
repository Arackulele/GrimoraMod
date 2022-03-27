namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameApparition = $"{GUID}_Apparition";

	private void Add_Apparition()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(GrimoraRandomAbility.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(4)
			.SetDescription("The amorphous Apparition. Its sigils are ever changing.")
			.SetNames(NameApparition, "Apparition")
			.Build();
	}
}
