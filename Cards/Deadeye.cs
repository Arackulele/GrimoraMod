using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadeye = $"{GUID}_Deadeye";

	private void Add_Deadeye()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Tutor)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("AN ANIMATED EYE, IT HELPS DRAW FORTH CREATURES FROM YOUR ARMY OF UNDEAD.")
			.SetNames(NameDeadeye, "Deadeye")
			.Build();
	}
}
