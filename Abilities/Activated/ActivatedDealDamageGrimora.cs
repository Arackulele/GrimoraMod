using DiskCardGame;

namespace GrimoraMod;

public class ActivatedDealDamageGrimora : ActivatedDealDamage
{
	public static Ability ability;

	public override Ability Ability => ability;

	public const int ENERGY_COST = 1;

	public override int EnergyCost => ENERGY_COST;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ActivatedDealDamageGrimora()
	{
		const string rulebookDescription = "Pay 1 Energy to deal 1 damage to the creature across from [creature].";

		ApiUtils.CreateAbility<ActivatedDealDamageGrimora>(rulebookDescription, "Soul Shot", true);
	}
}
