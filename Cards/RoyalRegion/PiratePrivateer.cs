using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePiratePrivateer = $"{GUID}_PiratePrivateer";

	private void Add_Card_PiratePrivateer()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Anchored.ability, Ability.Sniper)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetNames(NamePiratePrivateer, "Privateer")
			.Build();
	}
}
