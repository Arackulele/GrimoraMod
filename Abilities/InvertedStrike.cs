using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

public class InvertedStrike : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike the opposing slot as if the board was flipped. "
			+ "A card in the far left slot will attack the opposing far right slot.";

		return ApiUtils.CreateAbility<InvertedStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
