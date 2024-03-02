using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEmberSpirit = $"{GUID}_Ember_Spirit";

	private void Add_Card_EmberSpirit()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(FlameStrafe.ability, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(1, 3)
			.SetDescription("QUITE A MISCHIEVOUS SPIRIT, IT SPREADS ITS FLAMES TO CAUSE DISMAY.")
			.SetEnergyCost(6)
			.SetNames(NameEmberSpirit, "Ember Spirit")
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(FlameStrafe.ability, Ability.MadeOfStone)
			.SetBaseAttackAndHealth(1, 3)
			.SetDescription("QUITE A MISCHIEVOUS SPIRIT, IT SPREADS ITS FLAMES TO CAUSE DISMAY.")
			.SetEnergyCost(6)
			.SetNames(NameEmberSpirit, "Ember Spirit")
			.Build();
		}
	}
}
