using DiskCardGame;

namespace GrimoraMod;

public class AreaOfEffectStrike : StrikeAdjacentSlots
{
	public static Ability ability;
	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;

}

public partial class GrimoraPlugin
{
	public void Add_Ability_AreaOfEffectStrike()
	{
		const string rulebookDescription =
			"[creature] will strike its adjacent slots, and each opposing space to the left, right, and center of it.";

		AbilityBuilder<AreaOfEffectStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
