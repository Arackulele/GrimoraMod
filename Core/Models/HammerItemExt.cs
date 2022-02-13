using System.Collections;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class HammerItemExt : HammerItem
{
	private static readonly int Hit = Animator.StringToHash("hit");
	private int _useCounter = 0;
	private bool _dialoguePlayed = false;

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
						"I can't make it that easy for you! There's no fun if you destroy all this ice!"
					);
				}
				else
				{
					yield return targetSlot.Card.TakeDamage(100, null);
					_useCounter++;
				}
			}
		}

		if (!_dialoguePlayed && _useCounter == 1)
		{
			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput(
					"Don't get too comfortable with that, this Hammer is fragile and will break after the 3rd use!")
			);
		}
		else if (!_dialoguePlayed && _useCounter == 2)
		{
			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput("Getting carried away are we? You can only use it one more time.")
			);
		}
		else if (!_dialoguePlayed && _useCounter >= 3)
		{
			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput("The Hammer is now broken and you can no longer use it. I will have it fixed for the next battle though...")
			);
		}

		yield return new WaitForSeconds(0.65f);
	}

	public override bool ExtraActivationPrerequisitesMet()
	{
		return _useCounter < 3 && GetValidTargets().Count > 0;
	}
}
