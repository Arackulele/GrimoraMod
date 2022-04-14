using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public class Malnourishment : AbilityBehaviour
{
	public const string ModSingletonId = "GrimoraMod_Malnourishment";
	
	public static Ability ability;
	
	public override Ability Ability => ability;

	private CardModificationInfo _modInfo;
	
	private void Start()
	{
		_modInfo = new CardModificationInfo
		{
			singletonId = ModSingletonId
		};
	}

	public override bool RespondsToDealDamageDirectly(int amount) => amount > 0;

	public override IEnumerator OnDealDamageDirectly(int amount)
	{
		yield return PreSuccessfulTriggerSequence();
		
		Card.Anim.StrongNegationEffect();
		_modInfo.attackAdjustment -= 1;
		_modInfo.healthAdjustment -= 1;
		Card.Anim.StrongNegationEffect();

		yield return LearnAbility(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Malnourishment()
	{
		const string rulebookDescription = "Each time [creature] deals damage directly, it loses 1 power and health.";

		AbilityBuilder<Malnourishment>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
