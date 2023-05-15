using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGratefulDead = $"{GUID}_GratefulDead";

	private void Add_Card_GratefulDead()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(LatchBoneDigger.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("BOUND TO EARTH, THEY CLING ON SO THEY MAY ONE DAY GET PROPER BURIAL.")
			.SetNames(NameGratefulDead, "Grateful Dead")
			.Build();
	}
}
