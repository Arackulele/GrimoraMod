using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class GiantStrikeEnraged : GiantStrike
{
	public new static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GiantStrikeEnraged()
	{
		const string rulebookDescription = "[creature] will strike each opposing space.";

		ApiUtils.CreateAbility<GiantStrikeEnraged>(rulebookDescription, "Enraged Giant", flipYIfOpponent: true);
	}
}
