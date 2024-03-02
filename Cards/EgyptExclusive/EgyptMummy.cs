using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEgyptMummy = $"{GUID}_EgyptMummy";

	private void Add_Card_EgyptMummy()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Boneless.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetNames(NameEgyptMummy, "Lesser Mummy")
			.SetDescription("A loyal servant to the Pharao, buried with him to serve eternally in death.")
			.SetBoneCost(1)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Boneless.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetNames(NameEgyptMummy, "Lesser Mummy")
			.SetDescription("A loyal servant to the Pharao, buried with him to serve eternally in death.")
			.SetBoneCost(1)
			.Build();
		}
	}
}
