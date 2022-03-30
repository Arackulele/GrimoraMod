using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWechuge = $"{GUID}_Wechuge";

	private void Add_Card_Wechuge()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription("The vicious wechuge, corrupted by the spirit of an animal.")
			.SetNames(NameWechuge, "Wechuge")
			.Build();
	}
}
