using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameUndeadWolf = "ara_UndeadWolf";

	private void AddAra_UndeadWolf()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(3, 2)
			.WithBonesCost(4)
			.WithEnergyCost(4)
			.WithDescription("A diseased wolf. The pack has left him for death.")
			.WithNames(NameUndeadWolf, "Undead Wolf")
			.Build()
		);
	}
}