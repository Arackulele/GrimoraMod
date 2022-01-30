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
			Log.LogDebug($"[GrimoraModBattleSequencer] Player did not win...");
			AudioController.Instance.FadeOutLoop(3f, Array.Empty<int>());

			ViewManager.Instance.SwitchToView(View.Default, false, true);

			PlayerHand.Instance.PlayingLocked = true;

			InteractionCursor.Instance.InteractionDisabled = true;

			Log.LogDebug($"[GameEnd] Calling CardDrawPiles CleanUp...");
			StartCoroutine(CardDrawPiles.Instance.CleanUp());

			Log.LogDebug($"[GameEnd] Calling TurnManager CleanUp...");
			StartCoroutine(TurnManager.Instance.Opponent.CleanUp());

			yield return GlitchOutBoardAndHandCards();

			PlayerHand.Instance.SetShown(false, false);

			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Setting rulebook controller to not shown");
			RuleBookController.Instance.SetShown(shown: false);
			Log.LogDebug($"[GameEnd] Setting TableRuleBook.Instance enabled to false");
			TableRuleBook.Instance.enabled = false;
			Log.LogDebug($"[GameEnd] Glitching rulebook");
			GlitchOutAssetEffect.GlitchModel(TableRuleBook.Instance.transform, false, true);

			yield return new WaitForSeconds(0.75f);

			// yield return TextDisplayer.Instance.ShowUntilInput("Let the circle reset");

			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"RoyalBossDeleted",
				TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.5f);

			InteractionCursor.Instance.InteractionDisabled = false;

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
			GlitchOutAssetEffect.GlitchModel(GrimoraItemsManagerExt.Instance.HammerSlot.transform);
			yield return new WaitForSeconds(0.75f);

			// yield return (GameFlowManager.Instance as GrimoraGameFlowManager).EndSceneSequence();

			Log.LogDebug($"[GameEnd] Glitching bone tokens");
			(ResourcesManager.Instance as Part1ResourcesManager).GlitchOutBoneTokens();
			GlitchOutAssetEffect.GlitchModel(TableVisualEffectsManager.Instance.Table.transform);
			yield return new WaitForSeconds(0.75f);

			Log.LogDebug($"[GameEnd] Playing dialogue event");
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraFinaleEnd",
				TextDisplayer.MessageAdvanceMode.Input
			);

			Log.LogDebug($"[GameEnd] Switching to default view");
			ViewManager.Instance.SwitchToView(View.Default, immediate: false, lockAfter: true);

			Log.LogDebug($"[GameEnd] Time to rest");
			yield return TextDisplayer.Instance.ShowThenClear(
				"It is time to rest.", 2f, 0f, Emotion.Curious
			);
			yield return new WaitForSeconds(0.75f);
			Log.LogDebug($"[GameEnd] offset fov");
			ViewManager.Instance.OffsetFOV(150f, 1.5f);

			Log.LogDebug($"[GameEnd] Resetting running");
			yield return new WaitForSeconds(1f);
			ConfigHelper.ResetRun();
		}

		yield break;
	}

	public override List<CardInfo> GetFixedOpeningHand()
	{
		Log.LogDebug($"Getting randomized list for starting hand");
		var cardsToAdd = new List<CardInfo>();
		var randomizedChoices = GrimoraSaveData.Data.deck.Cards
			.ToArray()
			.Randomize()
			.ToList();

		while (cardsToAdd.Count < 3)
		{
			int seed = GenerateRandomSeed(randomizedChoices);

			var choice = randomizedChoices[seed];
			while (cardsToAdd.Contains(choice))
			{
				choice = randomizedChoices[GenerateRandomSeed(randomizedChoices)];
			}

			Log.LogDebug($"Adding random card choice [{choice.name}] to opening hand");
			cardsToAdd.Add(choice);
		}

		return cardsToAdd;
	}

	private static int GenerateRandomSeed(IReadOnlyCollection<CardInfo> randomizedChoices)
	{
		int seedRng = UnityEngine.Random.RandomRangeInt(int.MinValue, int.MaxValue);
		return SeededRandom.Range(
			0,
			randomizedChoices.Count,
			seedRng
		);
	}

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{
			Log.LogDebug($"[GrimoraModBattleSequencer] " +
			             $"Adding enemy [{ActiveEnemyPiece.name}] to config removed pieces");
			ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(ActiveEnemyPiece.name);
		}

		yield break;
	}

	private IEnumerator GlitchOutBoardAndHandCards()
	{
		yield return (BoardManager.Instance as BoardManager3D).HideSlots();
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
	}

	private static void GlitchOutCard(Card c)
	{
		(c.Anim as GravestoneCardAnimationController).PlayGlitchOutAnimation();
		UnityEngine.Object.Destroy(c.gameObject, 0.25f);
	}
}
