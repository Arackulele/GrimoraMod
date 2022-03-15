using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class HammerItemExt : HammerItem
{
	private static readonly int Glossiness = Shader.PropertyToID("_GlossMapScale");
	private static readonly int Hit = Animator.StringToHash("hit");

	private Material HammerHandleMat => transform.Find("Handle").GetComponent<MeshRenderer>().material;

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

		if (targetSlot.Card.IsNotNull())
		{
			if (_useCounter < 3)
			{
				if (TurnManager.Instance.Opponent is KayceeBossOpponent && targetSlot.CardIsNotNullAndHasAbility(Ability.IceCube))
				{
					ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = 2;
					_useCounter = 3;
				}

				yield return targetSlot.Card.Die(false);
				_useCounter++;
			}
		}

		if (_useCounter == 1 && ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue == 0)
		{
			yield return PlayDialogue(
				"THE FRAIL THING WILL SHATTER AFTER EXCESSIVE USE. THREE STRIKES, AND IT'S OUT, FOR THIS BATTLE AT LEAST."
			);
			HammerHandleMat.SetFloat(Glossiness, 0.6f);
			ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = 1;
		}
		else if (_useCounter == 2 && ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue == 1)
		{
			yield return PlayDialogue("GETTING CARRIED AWAY ARE WE? YOU CAN ONLY USE IT ONE MORE TIME.");
			HammerHandleMat.SetFloat(Glossiness, 1f);
			ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = 2;
		}
		else if (_useCounter >= 3 && ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue == 2)
		{
			yield return PlayDialogue(
				"THE HAMMER IS NOW BROKEN DEAR. I WILL HAVE IT FIXED FOR THE NEXT BATTLE THOUGH..."
			);
			gameObject.SetActive(false);
			GrimoraItemsManagerExt.Instance.hammerSlot.coll.enabled = false;
			ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = 3;
		}

		TextDisplayer.Instance.Clear();

		yield return new WaitForSeconds(0.65f);
	}


	public IEnumerator PlayDialogue(string dialogue)
	{
		if (ChessboardMapExt.Instance.HasNotPlayedDialogueOnce || ConfigHelper.Instance.HammerDialogueOption == 2)
		{
			yield return TextDisplayer.Instance.ShowThenClear(dialogue, 3f);
		}
	}

	public override bool ExtraActivationPrerequisitesMet()
	{
		return _useCounter < 3 && GetValidTargets().Count > 0;
	}
}

[HarmonyPatch(typeof(SelectableCardArray))]
public class FixCursorInteractionFromCorpseEaterTutorAfterHammeringACard
{
	[HarmonyPrefix, HarmonyPatch(nameof(SelectableCardArray.SelectCardFrom))]
	public static void SelectCardFromLogging(
		List<CardInfo> cards,
		CardPile pile,
		Action<SelectableCard> cardSelectedCallback,
		Func<bool> cancelCondition = null,
		bool forPositiveEffect = true
	)
	{
		if (InteractionCursor.Instance.InteractionDisabled)
		{
			GrimoraPlugin.Log.LogDebug($"Cursor interaction is disabled, re-enabling so you can select a card.");
			InteractionCursor.Instance.InteractionDisabled = false;
		}
	}
}
