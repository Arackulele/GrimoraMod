using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SeaLegs : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		return ApiUtils.CreateAbility<SeaLegs>(rulebookDescription);
	}
}
