using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModBattleSequencer : SpecialBattleSequencer
{
	public static ChessboardEnemyPiece ActiveEnemyPiece;

	private readonly List<CardInfo> _cardsThatHaveDiedThisMatch = new List<CardInfo>();

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

			yield return TextDisplayer.Instance.PlayDialogueEvent("GrimoraFinaleEnd", TextDisplayer.MessageAdvanceMode.Input);

			if (opponent is BaseBossExt ext)
			{
				Log.LogDebug($"[{GetType()}] Glitching mask and boss skull");
				yield return ext.HideBossSkull();
			}

			StartCoroutine(CardDrawPiles.Instance.CleanUp());

			StartCoroutine(opponent.CleanUp());

			yield return GlitchOutBoardAndHandCards();

			RuleBookController.Instance.SetShown(false);
			TableRuleBook.Instance.enabled = false;
			GlitchOutAssetEffect.GlitchModel(TableRuleBook.Instance.transform);
			yield return new WaitForSeconds(0.75f);

			GlitchOutAssetEffect.GlitchModel(ResourceDrone.Instance.transform);
			yield return new WaitForSeconds(0.75f);

			GlitchOutAssetEffect.GlitchModel(((BoardManager3D)BoardManager3D.Instance).Bell.transform);
			yield return new WaitForSeconds(0.75f);

			GlitchOutAssetEffect.GlitchModel(LifeManager.Instance.Scales3D.transform);
			yield return new WaitForSeconds(0.75f);

			GlitchOutAssetEffect.GlitchModel(GrimoraItemsManagerExt.Instance.hammerSlot.transform);
			yield return new WaitForSeconds(0.75f);

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

			yield return new WaitForSeconds(1f);
			ConfigHelper.Instance.ResetRun();
		}

		yield break;
	}

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
		PlayableCard killer)
	{
		return deathSlot.IsPlayerSlot;
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
		PlayableCard killer)
	{
		Log.LogDebug($"[GModBattleSequencer] Adding [{card.InfoName()}] to cardsThatHaveDiedThisGame. " +
		             $"Current count [{_cardsThatHaveDiedThisMatch.Count + 1}]");
		_cardsThatHaveDiedThisMatch.Add(card.Info);
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

		Log.LogDebug($"[GetFixedOpeningHand] Opening hand [{cardsToAdd.GetDelimitedString()}]");
		return cardsToAdd;
	}

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{
			// Log.LogDebug($"[GrimoraModBattleSequencer Adding enemy to config [{ActiveEnemyPiece.name}]");
			ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(ActiveEnemyPiece.name);
			_cardsThatHaveDiedThisMatch.Clear();
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
		(c.Anim as GravestoneCardAnimationController)?.PlayGlitchOutAnimation();
		Destroy(c.gameObject, 0.25f);
	}
}
