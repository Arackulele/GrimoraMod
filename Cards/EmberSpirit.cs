using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEmberSpirit = "GrimoraMod_Ember_Spirit";

	private void Add_Ember_spirit()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(FlameStrafe.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetDescription("A trickster spirit fleeing and leaving behind its flames.")
			.SetEnergyCost(6)
			.SetNames(NameEmberSpirit, "Spirit of Ember")
			.Build()
		);
	}
}
