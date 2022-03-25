using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class AreaOfEffectStrike : StrikeAdjacentSlots
{
	public static Ability ability;
	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike its adjacent slots, and each opposing space to the left, right, and center of it.";

		return ApiUtils.CreateAbility<AreaOfEffectStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
