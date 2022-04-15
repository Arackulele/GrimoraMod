using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class DrawSkeletonOnHit : AbilityBehaviour
{
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
		const string rulebookDescription =
			"Once [creature] is struck, draw a card from your Skeleton pile.";

		AbilityBuilder<DrawSkeletonOnHit>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Skeletons Within")
		 .Build();
	}
}
