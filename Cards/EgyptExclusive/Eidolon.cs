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
			.SetBaseAttackAndHealth(1, 4)
			.SetNames(NameEidolon, "Eidolon")
			.SetBoneCost(6)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Ability.Sentry, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(1, 4)
			.SetNames(NameEidolon, "Eidolon")
			.SetBoneCost(6)
			.Build();
		}
	}
}
