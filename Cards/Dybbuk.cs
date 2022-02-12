using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDybbuk = "GrimoraMod_Dybbuk";

	private void Add_Dybbuk()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Possessive.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(5)
			.SetDescription("No one knows what exactly the Dybbuk is, some say it is better left unknown.")
			.SetNames(NameDybbuk, "Dybbuk")
			.Build()
		);
	}
}
