using DiskCardGame;

namespace GrimoraMod;

public class ActivatedDealDamageGrimora : ActivatedDealDamage
{
	public const int ENERGY_COST = 1;
	
	public const string RulebookName = "Soul Shot";

	public static Ability ability;

	public override Ability Ability => ability;
	
	public override int EnergyCost => ENERGY_COST;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ActivatedDealDamageGrimora()
	{
		const string rulebookDescription = "Pay 1 Energy to deal 1 damage to the creature across from [creature].";

		AbilityBuilder<ActivatedDealDamageGrimora>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedDealDamageGrimora.RulebookName)
		 .Build();
	}
}
