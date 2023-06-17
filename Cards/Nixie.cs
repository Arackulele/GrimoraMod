using BepInEx.Bootstrap;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNixie = $"{GUID}_Nixie";

	private void Add_Card_Nixie()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LatchSubmerge.ability)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			.SetDescription("A troublesome lake spirit. It drags others down to a watery grave.")
			.SetNames(NameNixie, "Nixie")
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(LatchSubmerge.ability)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			.SetDescription("A troublesome lake spirit. It drags others down to a watery grave.")
			.SetNames(NameNixie, "Nixie")
			.Build();
		}
	}
}
