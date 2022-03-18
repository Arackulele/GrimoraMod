using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCompoundFracture = $"{GUID}_CompoundFracture";

	private static void Add_CompoundFracture()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Sharp)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(4)
			.SetNames(NameCompoundFracture, "Compound Fracture")
			.SetDescription("There will be quite a bit of difficulty putting it back togethe- OW!")
			.Build();
	}
}
