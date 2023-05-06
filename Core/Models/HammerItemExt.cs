using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class HammerItemExt : HammerItem
{
	private static readonly int Glossiness = Shader.PropertyToID("_GlossMapScale");
	private static readonly int Hit = Animator.StringToHash("hit");

	private Material HammerHandleMat => transform.Find("Handle").GetComponent<MeshRenderer>().material;

	public static int useCounter = 0;

	private void Start()
	{
		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.FrailHammer))		useCounter = 0;
		if(useCounter>=3)this.gameObject.SetActive(false);
		GameObject.Find("HammerModel").SetActive(false);
	}


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

		if (targetSlot.Card.NotDead() && useCounter < 3)
		{
			//Cant smash Frozen cards in KC
			if (TurnManager.Instance.Opponent is KayceeBossOpponent && targetSlot.Card.HasAbility(Ability.IceCube))
			{
				useCounter = 3;
			}



			//Cant kill Swashbucklers in Royal
			if (TurnManager.Instance.Opponent is RoyalBossOpponent && targetSlot.Card.HasAbility(Raider.ability))
			{
				useCounter = 3;
			}

			//Cant smash Frozen cards in KC
			if (TurnManager.Instance.Opponent is KayceeBossOpponent && targetSlot.Card.name == GrimoraPlugin.NameAvalanche)
			{
				useCounter = 3;
				targetSlot.Card.TakeDamage(1, null);

				yield return TextDisplayer.Instance.ShowUntilInput(
			"OH, HOW SAD. YOUR HAMMER COULD NOT BREAK THE ICE, AND SHATTERED"
				);

				yield break;
			}
			else yield return targetSlot.Card.Die(false);
			useCounter++;
		}

		switch (useCounter)
		{
			case 1 when !EventManagement.HasLearnedMechanicHammerSmashes:
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THE FRAIL THING WILL SHATTER AFTER EXCESSIVE USE. THREE STRIKES, AND IT'S OUT, FOR THIS BATTLE AT LEAST."
				);
				HammerHandleMat.SetFloat(Glossiness, 0.6f);
				EventManagement.HasLearnedMechanicHammerSmashes = true;
				break;
			case 2:
				HammerHandleMat.SetFloat(Glossiness, 1f);
				break;
			case >= 3:
				gameObject.SetActive(false);
				break;
		}

		TextDisplayer.Instance.Clear();

		yield return new WaitForSeconds(0.65f);
	}

	public override bool ExtraActivationPrerequisitesMet()
	{
		return useCounter < 3 && GetValidTargets().Count > 0;
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
