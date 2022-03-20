using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Raider : StrikeAdjacentSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	protected override Ability strikeAdjacentAbility => ability;

	public override bool RemoveDefaultAttackSlot() => true;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "[creature] will strike its adjacent slots, except other Raiders.";

		return ApiUtils.CreateAbility<Raider>(rulebookDescription);
	}
}
