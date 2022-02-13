using System.Collections;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class HammerItemExt : HammerItem
{
	internal int UseCounter = 0;
	private bool _dialoguePlayed = false;
	
	public override IEnumerator OnValidTargetSelected(CardSlot targetSlot, GameObject firstPersonItem)
	{
		firstPersonItem.GetComponentInChildren<Animator>().SetTrigger("hit");
		yield return new WaitForSeconds(0.166f);
		Tween.Position(firstPersonItem.transform, firstPersonItem.transform.position + Vector3.back * 4f, 0.15f, 0.2f, Tween.EaseOut, Tween.LoopType.None, null, delegate
		{
			firstPersonItem.gameObject.SetActive(value: false);
		});
		if (targetSlot.Card != null)
		{
			yield return targetSlot.Card.TakeDamage(100, null);
		}
		yield return new WaitForSeconds(0.65f);
		UseCounter++;
	}
	
	public override bool ExtraActivationPrerequisitesMet()
	{
		return UseCounter < 3;
	}

	public override void OnExtraActivationPrerequisitesNotMet()
	{
		if (!_dialoguePlayed)
		{
			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput("The was the last one, I hope you used it well.")
			);

			_dialoguePlayed = true;
		}
	}
}
