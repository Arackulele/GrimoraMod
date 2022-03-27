using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Anchored : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Anchored()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		ApiUtils.CreateAbility<Anchored>(rulebookDescription);
	}
}
