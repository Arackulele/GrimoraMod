using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class LooseLimb : TailOnHit
{
	public static Ability ability;
	
	public override Ability Ability => ability;
	
	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"When [creature] would be struck, a Tail is created in its place and [creature] moves into an adjacent open slot.";

		return ApiUtils.CreateAbility<LooseLimb>(rulebookDescription);
	}
}
