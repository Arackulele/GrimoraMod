using DiskCardGame;
using BepInEx.Bootstrap;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePiratePrivateer = $"{GUID}_PiratePrivateer";


	private void Add_Card_PiratePrivateer()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
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
		else
		{

			CardBuilder.Builder
		.SetAbilities(Anchored.ability, Ability.Sniper)
		.SetBaseAttackAndHealth(1, 1)
		.SetBoneCost(3)
		.SetDescription("A keen eye socket allows him to attack anywhere, from anywhere; marvelous indeed!")
		.SetNames(NamePiratePrivateer, "Privateer")
		.Build();




		}
	}
}
