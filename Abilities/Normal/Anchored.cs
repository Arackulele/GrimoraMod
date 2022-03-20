using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Anchored : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		return ApiUtils.CreateAbility<Anchored>(rulebookDescription);
	}
}
