using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEidolon = $"{GUID}_Eidolon";

	private void Add_Card_Eidolon()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Sentry, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(1, 3)
			.SetNames(NameEidolon, "Eidolon")
			.SetDescription("A GOLEM ENCHANTED LONG AGO, BOUND TO PROTECT THE AGE OF SUN.")
			.SetBoneCost(5)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Ability.Sentry, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(1, 3)
			.SetNames(NameEidolon, "Eidolon")
			.SetDescription("A GOLEM ENCHANTED LONG AGO, BOUND TO PROTECT THE AGE OF SUN.")
			.SetBoneCost(5)
			.Build();
		}
	}
}
