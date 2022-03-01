using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class GiantStrikeEnraged : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] will strike each opposing space.";

		return ApiUtils.CreateAbility<GiantStrikeEnraged>
			(rulebookDescription, "Enraged Giant", flipYIfOpponent: true);
	}
}
