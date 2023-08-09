using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class Malnourishment : AbilityBehaviour
{
	public const string ModSingletonId = "GrimoraMod_Malnourishment";

	public const string RulebookName = "Malnourishment";

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
		_modInfo.healthAdjustment -= 1;
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
		const string rulebookDescriptionEnglish = "Each time [creature] deals damage directly, it loses 1 power and health.";
		const string rulebookDescriptionChinese = "当[creature]直接造成伤害时，自身损失1点力量和生命。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Malnourishment>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Malnourishment.RulebookName)
		 .Build();
	}
}
