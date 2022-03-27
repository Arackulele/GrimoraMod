using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Boneless : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Boneless()
	{
		const string rulebookDescription = "[creature] yields no bones upon death.";

		ApiUtils.CreateAbility<Boneless>(rulebookDescription);
	}
}
