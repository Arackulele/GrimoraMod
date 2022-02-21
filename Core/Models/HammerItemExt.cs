using System.Collections;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class HammerItemExt : HammerItem
{
	private static readonly int Hit = Animator.StringToHash("hit");
	private int _useCounter = 0;

	public override IEnumerator OnValidTargetSelected(CardSlot targetSlot, GameObject firstPersonItem)
	{
		firstPersonItem.GetComponentInChildren<Animator>().SetTrigger(Hit);
		yield return new WaitForSeconds(0.166f);
		Tween.Position(
			firstPersonItem.transform,
			firstPersonItem.transform.position + Vector3.back * 4f,
			0.15f,
			0.2f,
			Tween.EaseOut,
			Tween.LoopType.None,
			null,
			delegate { firstPersonItem.gameObject.SetActive(false); }
		);

		if (targetSlot.Card != null)
		{
			if (_useCounter < 3)
			{
				if (TurnManager.Instance.Opponent is not null
				    && TurnManager.Instance.Opponent is KayceeBossOpponent
				    && targetSlot.Card.HasAbility(Ability.IceCube)
				   )
				{
					yield return TextDisplayer.Instance.ShowUntilInput(
						"I CAN'T MAKE IT THAT EASY FOR YOU! THERE'S NO FUN IF YOU DESTROY ALL THIS ICE!"
					);
				}
				else
				{
					yield return targetSlot.Card.TakeDamage(100, null);
					_useCounter++;
				}
			}
		}

		int hammerOption = ConfigHelper.Instance.HammerDialogueOption;
		if (hammerOption != 0)
		{
			bool hasNotPlayedOnce = hammerOption == 1 && ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue;
			if(hasNotPlayedOnce || hammerOption == 2)
			{
				if (_useCounter == 1)
				{
					StartCoroutine(
						TextDisplayer.Instance.ShowUntilInput("DON'T GET TOO ACCUSTOMED TO THAT HAMMER, DEAR.")
					);
					StartCoroutine(
						TextDisplayer.Instance.ShowUntilInput(
							"THE FRAIL THING WILL SHATTER AFTER EXCESSIVE USE. THREE STRIKES, AND IT'S OUT..")
					);
				}
				else if (_useCounter == 2)
				{
					StartCoroutine(
						TextDisplayer.Instance.ShowUntilInput("GETTING CARRIED AWAY ARE WE? YOU CAN ONLY USE IT ONE MORE TIME.")
					);
				}
				else if (_useCounter >= 3)
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						"THE HAMMER IS NOW BROKEN AND YOU CAN NO LONGER USE IT. I WILL HAVE IT FIXED FOR THE NEXT BATTLE THOUGH..."
					));
					ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = false;
				}
			}
		}

		yield return new WaitForSeconds(0.65f);
	}

	public override bool ExtraActivationPrerequisitesMet()
	{
		return _useCounter < 3 && GetValidTargets().Count > 0;
	}
}
