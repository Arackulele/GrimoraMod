using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWechuge = $"{GUID}_Wechuge";

	private void Add_Card_Wechuge()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DoubleStrike, Ability.MoveBeside)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription("Corrupted by the spirit of a rabid wolf. It's fury cannot be stopped.")
			.SetNames(NameWechuge, "Wechuge")
			.Build();
	}
}
