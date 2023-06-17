using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadeye = $"{GUID}_Deadeye";

	private void Add_Card_Deadeye()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Tutor)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("SOME SAY THIS EYE ONCE BELONGED TO AN ANCIENT GOD, IT GAZES UPON YOUR ARMY, PICKING ITS FAVORITE TO AID IT ON THE BATTLEFIELD.")
			.SetNames(NameDeadeye, "Deadeye")
			.Build();
	}
}
