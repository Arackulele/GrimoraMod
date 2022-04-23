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
		 .SetBoneCost(3)
		 .SetDescription("A keen eye socket allows him to attack anywhere, from anywhere; marvelous indeed!")
		 .SetNames(NamePiratePrivateer, "Privateer")
		 .Build();
	}
}
