using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCompoundFracture = $"{GUID}_CompoundFracture";

	private void Add_Card_CompoundFracture()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Sharp)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(4)
			.SetNames(NameCompoundFracture, "Compound Fracture")
			.SetDescription("CRUSHED BY THE ROOF OF HIS OWN HOUSE. LIVING ON AS A PILE OF GOO.")
			.Build();
	}
}
