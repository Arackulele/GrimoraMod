using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModBattleSequencer : SpecialBattleSequencer
{
	public static ChessboardEnemyPiece ActiveEnemyPiece;

	public override IEnumerator PreCleanUp()
	{
		if (!TurnManager.Instance.PlayerIsWinner())
		{
			Opponent opponent = TurnManager.Instance.Opponent;

			Log.LogDebug($"[PreCleanUp] Player did not win...");
			AudioController.Instance.FadeOutLoop(3f, Array.Empty<int>());

			ViewManager.Instance.SwitchToView(View.Default, false, true);

			PlayerHand.Instance.PlayingLocked = true;

			InteractionCursor.Instance.InteractionDisabled = true;

			yield return TextDisplayer.Instance.PlayDialogueEvent("RoyalBossDeleted", TextDisplayer.MessageAdvanceMode.Input);
			yield return new WaitForSeconds(0.5f);

			Log.LogDebug($"[GameEnd] Playing dialogue event");
			yield return TextDisplayer.Instance.PlayDialogueEvent("GrimoraFinaleEnd", TextDisplayer.MessageAdvanceMode.Input);

			if (opponent is BaseBossExt ext && ext.Mask != null)
			{
				Log.LogDebug($"[{GetType()}] Glitching mask and boss skull");
				GlitchOutAssetEffect.GlitchModel(ext.Mask.transform, true);
				GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");
				GlitchOutAssetEffect.GlitchModel(ext.bossSkull.transform, true);
			}

			Log.LogDebug($"[PreCleanUp] Calling CardDrawPiles CleanUp...");
			StartCoroutine(CardDrawPiles.Instance.CleanUp());

			Log.LogDebug($"[PreCleanUp] Calling TurnManager CleanUp...");
			StartCoroutine(opponent.CleanUp());

			yield return GlitchOutBoardAndHandCards();

			Log.LogDebug($"[PreCleanUp] Setting rulebook controller to not shown");
			RuleBookController.Instance.SetShown(false);
			Log.LogDebug($"[PreCleanUp] Setting TableRuleBook.Instance enabled to false");
			TableRuleBook.Instance.enabled = false;
			Log.LogDebug($"[PreCleanUp] Glitching rulebook");
			GlitchOutAssetEffect.GlitchModel(TableRuleBook.Instance.transform);
			yield return new WaitForSeconds(0.75f);


			Log.LogDebug($"[GameEnd] Glitching Resource Energy");
			GlitchOutAssetEffect.GlitchModel(ResourceDrone.Instance.transform);
			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Glitching bell");
			GlitchOutAssetEffect.GlitchModel(((BoardManager3D)BoardManager3D.Instance).Bell.transform);
			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Glitching scales");
			GlitchOutAssetEffect.GlitchModel(LifeManager.Instance.Scales3D.transform);
			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Glitching hammer");
			GlitchOutAssetEffect.GlitchModel(GrimoraItemsManagerExt.Instance.hammerSlot.transform);
			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Glitching bone tokens");
			(ResourcesManager.Instance as Part1ResourcesManager).GlitchOutBoneTokens();
			GlitchOutAssetEffect.GlitchModel(TableVisualEffectsManager.Instance.Table.transform);
			yield return new WaitForSeconds(0.75f);

			InteractionCursor.Instance.InteractionDisabled = false;

			Log.LogDebug($"[GameEnd] Switching to default view");
			ViewManager.Instance.SwitchToView(View.Default, false, true);

			Log.LogDebug($"[GameEnd] Time to rest");
			yield return TextDisplayer.Instance.ShowThenClear(
				"It is time to rest.", 2f, 0f, Emotion.Curious
			);
			yield return new WaitForSeconds(0.75f);
			Log.LogDebug($"[GameEnd] offset fov");
			ViewManager.Instance.OffsetFOV(150f, 1.5f);

			Log.LogDebug($"[GameEnd] Resetting run");
			yield return new WaitForSeconds(1f);
			ConfigHelper.Instance.ResetRun();
		}

		yield break;
	}

	public override List<CardInfo> GetFixedOpeningHand()
	{
		Log.LogDebug($"[GetFixedOpeningHand] Getting randomized list for starting hand");
		var cardsToAdd = new List<CardInfo>();
		var gravedigger = GrimoraSaveUtil.DeckList.Find(info => info.name.Equals(NameGravedigger));
		var bonepile = GrimoraSaveUtil.DeckList.Find(info => info.name.Equals(NameBonepile));
		if (bonepile is not null)
		{
			cardsToAdd.Add(bonepile);
		}
		else if (gravedigger is not null)
		{
			cardsToAdd.Add(gravedigger);
		}

		var randomizedChoices = RandomUtils.GenerateRandomChoicesOfCategory(
			GrimoraSaveUtil.DeckList,
			RandomUtils.GenerateRandomSeed(GrimoraSaveUtil.DeckList)
		);

		if (randomizedChoices.Count < 3)
		{
			Log.LogWarning($"[GetFixedOpeningHand] ...How did you get fewer than 3 cards in your deck?!");
			randomizedChoices.AddRange(new[]
			{
				new CardChoice() { info = "Skeleton".GetCardInfo() }, new CardChoice() { info = "Skeleton".GetCardInfo() }
			});
		}

		while (cardsToAdd.Count < 3)
		{
			cardsToAdd.Add(randomizedChoices[UnityEngine.Random.RandomRangeInt(0, randomizedChoices.Count)].info);
		}
		
		Log.LogDebug($"[GetFixedOpeningHand] Opening hand [{cardsToAdd.GetDelimitedString()}]");
		return cardsToAdd;
	}

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{
			// Log.LogDebug($"[GrimoraModBattleSequencer Adding enemy to config [{ActiveEnemyPiece.name}]");
			ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(ActiveEnemyPiece.name);
		}

		yield break;
	}

	private IEnumerator GlitchOutBoardAndHandCards()
	{
		yield return ((BoardManager3D)BoardManager.Instance).HideSlots();
		foreach (PlayableCard c in BoardManager.Instance.CardsOnBoard)
		{
			GlitchOutCard(c);
			yield return new WaitForSeconds(0.1f);
		}

		// List<PlayableCard>.Enumerator enumerator = default(List<PlayableCard>.Enumerator);
		foreach (PlayableCard c2 in PlayerHand.Instance.CardsInHand)
		{
			GlitchOutCard(c2);
			yield return new WaitForSeconds(0.1f);
		}

		PlayerHand.Instance.SetShown(false);

		yield return new WaitForSeconds(0.75f);
	}

	private static void GlitchOutCard(Card c)
	{
		((GravestoneCardAnimationController)c.Anim).PlayGlitchOutAnimation();
		Destroy(c.gameObject, 0.25f);
	}
}
