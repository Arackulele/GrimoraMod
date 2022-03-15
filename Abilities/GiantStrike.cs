using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class GiantStrike : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike each opposing space. "
			+ "If only one creature is in the opposing spaces, this card will strike that creature twice. ";

		return ApiUtils.CreateAbility<GiantStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
