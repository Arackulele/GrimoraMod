using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SkinCrawler : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	private PlayableCard hidingUnderCard;

	public override bool RespondsToResolveOnBoard()
	{
		return !base.Card.OpponentCard;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		// CardSlot toLeft = BoardManager.Instance.GetAdjacent(base.Card.Slot, adjacentOnLeft: true);
		// Log.LogDebug($"[SkinCrawler] Checking if adj slots from [{base.Card.Slot}] are not null");
		CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, adjacentOnLeft: true);
		CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, adjacentOnLeft: false);
		if (toRightSlot is not null && toRightSlot.Card is not null)
		{
			Transform toRightTransform = toRightSlot.Card.transform;
			Transform toRightSlotTransform = toRightSlot.Card.Slot.transform;

			ViewManager.Instance.SwitchToView(View.Board);

			yield return new WaitForSeconds(0.4f);
			// to the left and up, like something is being added under it
			Vector3 vector = new Vector3(0f, 0.25f, 0f);
			// do to card that will be hiding Boo Hag
			Tween.Position(toRightTransform, toRightSlotTransform.position + vector, 0.1f, 0f, Tween.EaseInOut);

			Vector3 cardRot = toRightSlot.Card.transform.rotation.eulerAngles;

			// rotate on z-axis, as if you rotated your hand holding the card counter-clockwise
			// Tween.Rotate(toRightTransform, new Vector3(0f, 0f, 0f), Space.World, 0.1f, 0f, Tween.EaseInOut);

			// GameObject gameObject = Object.Instantiate(ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CardPack"));


			// Vector3 positionFurtherAwayFromBaseCard = toRightSlotTransform.position + Vector3.forward * 8f;
			// set starting position 
			// base.Card.transform.position = positionFurtherAwayFromBaseCard;

			// move pack from current position to the baseCardSlotPosition
			// Log.LogDebug($"[SkinCrawler] moving BooHag to [{toRightSlotTransform.position}]");
			TweenBase tween = Tween.Position(
				base.Card.transform,
				toRightSlot.Card.Slot.transform.position + new Vector3(0f, 0f, 0.31f),
				0.4f,
				0f,
				Tween.EaseOut
			);

			while (tween.Status is not Tween.TweenStatus.Finished)
			{
				// Log.LogDebug($"[SkinCrawler] playing negation effect in loop");
				toRightSlot.Card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.1f);
			}

			yield return new WaitForSeconds(0.1f);

			// rotate base card with it's original rotation values so that it lays flat on the board again
			// Log.LogDebug($"[SkinCrawler] rotating [{toRightSlot.Card.Info.name}]");
			Tween.Rotation(toRightTransform, cardRot, 0.1f, 0f, Tween.EaseInOut);

			// offset the card to be a little higher
			// Log.LogDebug($"[SkinCrawler] Setting height offset");
			toRightSlot.Card.SlotHeightOffset = 0.13f;

			// reassign the card to the slot
			Log.LogDebug($"[SkinCrawler] Assigning [{toRightSlot.Card.Info.name}] to slot");
			yield return BoardManager.Instance.AssignCardToSlot(toRightSlot.Card, toRightSlot.Card.Slot);

			toRightSlot.Card.Anim.Anim.StartPlayback();
			yield return new WaitForSeconds(0.25f);
			// Log.LogDebug($"[SkinCrawler] Adding temporary mod");
			toRightSlot.Card.AddTemporaryMod(new CardModificationInfo() { attackAdjustment = 1, healthAdjustment = 1 });

			yield return new WaitUntil(() =>
				!Tween.activeTweens.Exists((TweenBase t) => t.targetInstanceID == toRightSlot.Card.transform.GetInstanceID())
			);

			Log.LogDebug($"[SkinCrawler] Setting Boo Hag as child of card");
			base.transform.SetParent(toRightSlot.Card.transform); // now a child of the playable card
			Log.LogDebug($"[SkinCrawler] Setting Boo Hag slot [{base.Card.Slot.Index}] to null");
			BoardManager.Instance.playerSlots[base.Card.Slot.Index].Card = null;
			Log.LogDebug($"[SkinCrawler] Setting Boo Hag slot to [{toRightSlot.Card.Slot}]");
			base.Card.Slot = toRightSlot.Card.Slot;

			hidingUnderCard = toRightSlot.Card;
			// base.gameObject.SetActive(false);}
		}
	}

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
		PlayableCard killer)
	{
		// Log.LogDebug($"[SkinCrawler] Checking if Boo Hag should respond to other card dying.");
		return hidingUnderCard != null && card.Slot == hidingUnderCard.Slot;
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
		PlayableCard killer)
	{
		hidingUnderCard = null;
		base.Card.Anim.StrongNegationEffect();
		Log.LogDebug($"[SkinCrawler] Resolving [{base.Card}] to deathSlot [{deathSlot.Index}]");
		CardInfo infoCopy = base.Card.Info;
		Log.LogDebug($"[SkinCrawler] Destroying existing card");
		Object.Destroy(base.Card.gameObject);
		Log.LogDebug($"[SkinCrawler] Creating new card");
		yield return BoardManager.Instance.CreateCardInSlot(infoCopy, deathSlot, 0f);
		// BoardManager.Instance.playerSlots[deathSlot.Index].Card = base.Card;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil and, if possible," +
			" hide under the card providing a +1 buff";

		return ApiUtils.CreateAbility<SkinCrawler>(rulebookDescription);
	}
}

[HarmonyPatch(typeof(BoardManager))]
public class SkinCrawlerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager.CardsOnBoard), MethodType.Getter)]
	public static void Postfix(ref List<PlayableCard> __result)
	{
		List<PlayableCard> newList = new List<PlayableCard>(__result);
		foreach (var card in __result)
		{
			for (int i = 0; i < card.transform.childCount; i++)
			{
				SkinCrawler skinCrawler = card.transform.GetChild(i).GetComponent<SkinCrawler>();
				if (skinCrawler is not null)
				{
					// now we get the card itself to add to the list
					newList.Add(skinCrawler.GetComponent<PlayableCard>());
				}
			}
		}
		__result = newList;
	}
}
