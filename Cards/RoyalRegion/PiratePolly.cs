using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePiratePolly = $"{GUID}_PiratePolly";

	private void Add_Card_PiratePolly()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle, Ability.Flying)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(4)
			.SetNames(NamePiratePolly, "Polly")
			.Build();
	}
}
