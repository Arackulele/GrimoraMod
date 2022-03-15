using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class InvertedStrike : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike the opposing slot as if the board was flipped. "
			+ "A card in the far left slot will attack the opposing far right slot.";

		return ApiUtils.CreateAbility<InvertedStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
