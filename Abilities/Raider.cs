using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class Raider : StrikeAdjacentSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	protected override Ability strikeAdjacentAbility => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] will strike it's adjacent slots.";

		return ApiUtils.CreateAbility<Raider>(rulebookDescription);
	}
}
