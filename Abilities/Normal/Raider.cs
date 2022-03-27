using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Raider : StrikeAdjacentSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;

	public override bool RemoveDefaultAttackSlot() => true;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Raider()
	{
		const string rulebookDescription = "[creature] will strike its adjacent slots, except other Raiders.";

		ApiUtils.CreateAbility<Raider>(rulebookDescription);
	}
}
