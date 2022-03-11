using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(BoardManager))]
public class BoardManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager.QueueCardForSlot))]
	public static void PostfixAssignAsChildForQueuedCard(
		PlayableCard card,
		CardSlot slot,
		float tweenLength = 0.1f,
		bool doTween = true,
		bool setStartPosition = true
	)
	{
		card.transform.SetParent(BoardManager.Instance.OpponentQueueSlots[card.QueuedSlot.Index].transform);
		if (TurnManager.Instance.Opponent is RoyalBossOpponentExt && !card.HasAbility(SeaLegs.ability))
		{
			CardInfo copyInfo = card.Info.Clone() as CardInfo;
			copyInfo.Mods.Add(new CardModificationInfo(SeaLegs.ability));

			card.SetInfo(copyInfo);
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(BoardManager.ResolveCardOnBoard))]
	public static void AddSeaLegsToCardsThatDontHaveItForRoyal(
		PlayableCard card,
		CardSlot slot,
		float tweenLength = 0.1f,
		Action landOnBoardCallback = null,
		bool resolveTriggers = true
	)
	{
		if (TurnManager.Instance.Opponent is RoyalBossOpponentExt && !card.HasAbility(SeaLegs.ability))
		{
			CardInfo copyInfo = card.Info.Clone() as CardInfo;
			copyInfo.Mods.Add(new CardModificationInfo(SeaLegs.ability));

			card.SetInfo(copyInfo);
		}
	}
}
