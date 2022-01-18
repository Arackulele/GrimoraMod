using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEmberSpirit = "ara_Ember_Spirit";

	private void AddAra_Ember_spirit()
	{
		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(FlameStrafe.ability)
			.WithBaseAttackAndHealth(1, 3)
			.WithBonesCost(3)
			.WithDescription("A trickster spirit fleeing and leaving behind its flames.")
			.WithEnergyCost(3)
			.WithNames(NameEmberSpirit, "Spirit of Ember")
			.Build()
		);
	}
}