using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class SkinCrawler : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;


	public static NewAbility Create()
	{
		const string rulebookDescription =
			"At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil and, if possible," +
			" hide under the card providing a +1 buff";

		return ApiUtils.CreateAbility<SkinCrawler>(rulebookDescription);
	}
}
