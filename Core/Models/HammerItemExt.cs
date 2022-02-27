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

	private readonly int _hammerOption = ConfigHelper.Instance.HammerDialogueOption;

	private bool HasNotPlayedDialogueOnce =>
		_hammerOption == 1 && ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue;

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
					yield return targetSlot.Card.Die(false);
					_useCounter++;
				}
			}
		}

		if (_useCounter == 1)
		{
			yield return PlayDialogue("THE FRAIL THING WILL SHATTER AFTER EXCESSIVE USE. THREE STRIKES, AND IT'S OUT, FOR THIS BATTLE AT LEAST.");
			HammerHandleMat.SetFloat(Glossiness, 0.6f);
		}
		else if (_useCounter == 2)
		{
			yield return PlayDialogue("GETTING CARRIED AWAY ARE WE? YOU CAN ONLY USE IT ONE MORE TIME.");
			HammerHandleMat.SetFloat(Glossiness, 1f);
		}
		else if (_useCounter >= 3)
		{
			yield return PlayDialogue(
				"THE HAMMER IS NOW BROKEN DEAR. I WILL HAVE IT FIXED FOR THE NEXT BATTLE THOUGH..."
			);
			ChessboardMapExt.Instance.hasNotPlayedAllHammerDialogue = false;
			gameObject.SetActive(false);
		}
		TextDisplayer.Instance.Clear();

		yield return new WaitForSeconds(0.65f);
	}


	public IEnumerator PlayDialogue(string dialogue)
	{
		if (HasNotPlayedDialogueOnce || _hammerOption == 2)
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
public class FixCursorInteractionWithHammer
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
