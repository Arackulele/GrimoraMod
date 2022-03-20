using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Boneless : AbilityBehaviour
{
	public static Ability ability;
	
	public override Ability Ability => ability;
	
	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "[creature] yields no bones upon death.";

		return ApiUtils.CreateAbility<Boneless>(rulebookDescription);
	}
}
