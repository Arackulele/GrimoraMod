using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class DrawSkeletonOnHit : AbilityBehaviour
{
	public const string RulebookName = "Skeletons Within";

	public static Ability ability;
	
	public override Ability Ability => ability;

	public override bool RespondsToTakeDamage(PlayableCard source)
	{
		return CardDrawPiles3D.Instance.SideDeck.CardsInDeck > 0;
	}

	public override IEnumerator OnTakeDamage(PlayableCard source)
	{
		yield return PreSuccessfulTriggerSequence();
		Card.Anim.StrongNegationEffect();
		yield return new WaitForSeconds(0.4f);
		if (ViewManager.Instance.CurrentView != View.Default)
		{
			yield return new WaitForSeconds(0.2f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(0.2f);
		}
		CardDrawPiles3D.Instance.SidePile.Draw();
		yield return CardDrawPiles3D.Instance.DrawFromSidePile();
		yield return LearnAbility(0.5f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_DrawSkeletonOnHit()
	{
		const string rulebookDescriptionEnglish =
			"Once [creature] is struck, draw a card from your Skeleton pile.";
		const string rulebookDescriptionChinese =
			"[creature]受到攻击时，从骷髅副牌组中抽一张牌。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<DrawSkeletonOnHit>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(DrawSkeletonOnHit.RulebookName)
		 .Build();
	}
}
