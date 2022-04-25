using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWechuge = $"{GUID}_Wechuge";

	private void Add_Card_Wechuge()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DoubleStrike)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription("Corrupted by the spirit of an animal. It will make furious swipes with each hand.")
			.SetNames(NameWechuge, "Wechuge")
			.Build();
	}
}
