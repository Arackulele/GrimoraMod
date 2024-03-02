using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneless = $"{GUID}_Boneless";

	private void Add_Card_Boneless()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			 .SetAsNormalCard()
			.SetAbilities(Boneless.ability, GainAttackNoBones.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetNames(NameBoneless, "Boneless")
			.SetDescription("It's Skeleton seems to have left this lifeless husk in order to achieve greater things.")
			.SetBoneCost(4)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Boneless.ability, GainAttackNoBones.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetNames(NameBoneless, "Boneless")
			.SetDescription("It's Skeleton seems to have left this lifeless husk in order to achieve greater things.")
			.SetBoneCost(4)
			.Build();
		}
	}
}
