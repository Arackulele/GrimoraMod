using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class GiantStrike : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike each opposing space that is occupied by a creature. "
			+ "If only one creature is in the opposing spaces, this card will strike twice. "
			+ "This card will strike directly if no creatures oppose it.";

		return ApiUtils.CreateAbility<GiantStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
