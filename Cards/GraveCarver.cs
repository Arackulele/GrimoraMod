using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGraveCarver = $"{GUID}_GraveCarver";

	private void Add_Card_GraveCarver()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Sculptor.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(5)
			.SetDescription("HE HAS SPENT HIS LIFE CARVING THESE CREATURES, THIS CARD CONTAINS A PART OF HIS SOUL.")
			.SetNames(NameGraveCarver, "Grave Carver")
			.Build();
	}
}
