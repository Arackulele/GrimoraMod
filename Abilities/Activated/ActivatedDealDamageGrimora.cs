using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class ActivatedDealDamageGrimora : ActivatedDrawSkeleton
{
	public static readonly Ability ability;

	public override Ability Ability => ability;

	public const int ENERGY_COST = 1;
	
	public override int EnergyCost => ENERGY_COST;

	public static AbilityManager.FullAbility Create()
	{
		string rulebookDescription = $"Pay {ENERGY_COST} Energy to deal 1 damage to the creature across from [creature].";

		return ApiUtils.CreateAbility<ActivatedDealDamageGrimora>(rulebookDescription, "Soul Shot", true);
	}
}
