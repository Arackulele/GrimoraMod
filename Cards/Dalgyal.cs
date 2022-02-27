using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDalgyal = "GrimoraMod_Dalgyal";

	private void Add_Dalgyal()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Sentry)
			.SetBaseAttackAndHealth(0, 2)
			.SetEnergyCost(2)
			.SetDescription("A SPIRIT THAT TAKES THE FORM OF AN EGG. ITS PRESENCE HARMS THOSE THAT GAZE UPON IT.")
			.SetNames(NameDalgyal, "Dalgyal")
			.Build();
	}
}

