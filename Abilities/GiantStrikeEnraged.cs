using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class GiantStrikeEnraged : GiantStrike
{
	public new static Ability ability;

	public override Ability Ability => ability;

	public new static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "[creature] will strike each opposing space.";

		return ApiUtils.CreateAbility<GiantStrikeEnraged>(rulebookDescription, "Enraged Giant", flipYIfOpponent: true);
	}
}
