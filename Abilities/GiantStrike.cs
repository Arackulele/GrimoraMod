using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class GiantStrike : AbilityBehaviour
{
	public static readonly NewAbility NewAbility = Create();

	public static Ability ability;

	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] will strike each opposing space.";

		return ApiUtils.CreateAbility<GiantStrike>(rulebookDescription);
	}
}
