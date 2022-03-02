using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class AreaOfEffectStrike : StrikeAdjacentSlots
{
	public static Ability ability;
	public override Ability Ability => ability;

	protected override Ability strikeAdjacentAbility => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike it's adjacent slots, and each opposing space to the left, right, and center of it.";

		return ApiUtils.CreateAbility<AreaOfEffectStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
