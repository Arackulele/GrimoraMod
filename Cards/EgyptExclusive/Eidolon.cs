using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEidolon = $"{GUID}_Eidolon";

	private void Add_Card_Eidolon()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Sentry, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(2, 4)
			.SetNames(NameEidolon, "Eidolon")
			.SetBoneCost(7)
			.Build();
	}
}
