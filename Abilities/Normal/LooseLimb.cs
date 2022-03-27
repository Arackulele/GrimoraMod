using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class LooseLimb : TailOnHit
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LooseLimb()
	{
		const string rulebookDescription =
			"When [creature] would be struck, a Tail is created in its place and this card moves into an adjacent open slot.";

		ApiUtils.CreateAbility<LooseLimb>(rulebookDescription);
	}
}
