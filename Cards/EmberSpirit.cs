using BepInEx.Bootstrap;

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
			.SetAbilities(FlameStrafe.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetDescription("A TRICKSTER SPIRIT FLEEING AND LEAVING BEHIND ITS FLAMES.")
			.SetEnergyCost(6)
			.SetNames(NameEmberSpirit, "Spirit of Ember")
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(FlameStrafe.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetDescription("A TRICKSTER SPIRIT FLEEING AND LEAVING BEHIND ITS FLAMES.")
			.SetEnergyCost(6)
			.SetNames(NameEmberSpirit, "Spirit of Ember")
			.Build();
		}
	}
}
