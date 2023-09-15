using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

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
		Card.AddTemporaryMod(_modInfo);
	}

	public override bool RespondsToDealDamageDirectly(int amount) => Card.NotDead() && amount > 0;

	public override IEnumerator OnDealDamageDirectly(int amount)
	{
		yield return PreSuccessfulTriggerSequence();
		
		Card.Anim.StrongNegationEffect();
		_modInfo.attackAdjustment -= 1;
		Card.TakeDamage(1, null);
		Card.Anim.StrongNegationEffect();

		yield return LearnAbility(0.25f);

		if (Card.Health <= 0)
		{
			yield return Card.Die(false);
		}
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
