using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneclaw = $"{GUID}_Boneclaw";

	private void Add_Card_Boneclaw()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Slasher.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameBoneclaw, "Boneclaw")
			.SetBoneCost(7)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Slasher.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameBoneclaw, "Boneclaw")
			.SetBoneCost(7)
			.Build();
		}
	}
}
