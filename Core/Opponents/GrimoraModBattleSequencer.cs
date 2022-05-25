using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModBattleSequencer : SpecialBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModBattleSequencer),
		typeof(GrimoraModBattleSequencer)
	);

	public static ChessboardEnemyPiece ActiveEnemyPiece;

	private readonly List<CardInfo> _cardsThatHaveDiedThisMatch = new List<CardInfo>();

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		EncounterData data = new EncounterData
		{
			opponentType = Opponent.Type.Default,
			opponentTurnPlan = nodeData.blueprint
			 .turns.Select(bpList1 => bpList1.Select(bpList2 => bpList2.card).ToList())
			 .ToList()
		};
		data.opponentType = this is GrimoraModBossBattleSequencer boss ? boss.BossType : GrimoraModFinaleOpponent.FullOpponent.Id;

		return data;
	}

	public override IEnumerator PreCleanUp()
	{
		if (!TurnManager.Instance.PlayerIsWinner() && ! AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.InfinitLives))
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

			StartCoroutine(CardDrawPiles.Instance.CleanUp());

			StartCoroutine(opponent.CleanUp());

			yield return GlitchOutBoardAndHandCards();

			RuleBookController.Instance.SetShown(false);
			TableRuleBook.Instance.enabled = false;
			GlitchOutAssetEffect.GlitchModel(TableRuleBook.Instance.transform);
			yield return new WaitForSeconds(0.5f);

			GlitchOutAssetEffect.GlitchModel(ResourceDrone.Instance.transform);
			yield return new WaitForSeconds(0.5f);

			GlitchOutAssetEffect.GlitchModel(((BoardManager3D)BoardManager3D.Instance).Bell.transform);
			yield return new WaitForSeconds(0.5f);

			GlitchOutAssetEffect.GlitchModel(LifeManager.Instance.Scales3D.transform);
			yield return new WaitForSeconds(0.5f);

			GlitchOutAssetEffect.GlitchModel(GrimoraItemsManagerExt.Instance.hammerSlot.transform);
			yield return new WaitForSeconds(0.5f);

			(ResourcesManager.Instance as Part1ResourcesManager).GlitchOutBoneTokens();
			GlitchOutAssetEffect.GlitchModel(TableVisualEffectsManager.Instance.Table.transform);
			yield return new WaitForSeconds(0.5f);

			if (FindObjectOfType<StinkbugInteractable>())
			{
				FindObjectOfType<StinkbugInteractable>().OnCursorSelectStart();
			}

			GlitchOutAssetEffect.GlitchModel(GameObject.Find("EntireChamber").transform);
			yield return new WaitForSeconds(0.5f);

			InteractionCursor.Instance.InteractionDisabled = false;

			Log.LogDebug($"[GameEnd] Switching to default view");
			ViewManager.Instance.SwitchToView(View.Default, false, true);

			Log.LogDebug($"[GameEnd] Time to rest");
			yield return TextDisplayer.Instance.ShowThenClear(
				"It is time to rest.",
				2f,
				0f,
				Emotion.Curious
			);
			yield return new WaitForSeconds(0.75f);
			Log.LogDebug($"[GameEnd] offset fov");
			ViewManager.Instance.OffsetFOV(150f, 1.5f);

			yield return new WaitForSeconds(1f);
			AudioController.Instance.StopAllLoops();
			AudioController.Instance.StopAllCoroutines();
			yield return MenuController.Instance.TransitionToGame2(true);
		}
	}

	public override IEnumerator PlayerUpkeep()
	{
		if (SaveFile.IsAscension)
		{
			Log.LogInfo($"[BattleSequencer.OnUpkeep] Is Ascension and Is player upkeep");
			if (TurnManager.Instance.TurnNumber % 3 == 0 && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.SawyersShowdown))
			{
				yield return HandleSawyersShowdownChallenge();
			}

			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.KayceesKerfuffle))
			{
				switch (TurnManager.Instance.TurnNumber)
				{
					case 3:
					{
						yield return TextDisplayer.Instance.ShowUntilInput("I hope you're able to warm up next turn.");
						break;
					}
					case 4:
						yield return HandleKayceeKerfuffleChallenge();
						break;
				}
			}
		}
	}

	private IEnumerator HandleSawyersShowdownChallenge()
	{
		if (ResourcesManager.Instance.PlayerBones > 0)
		{
			ViewManager.Instance.SwitchToView(View.BoneTokens);
			yield return new WaitForSeconds(0.25f);
			yield return TextDisplayer.Instance.ShowUntilInput("Sawyer thanks you for your contribution!");
			yield return ResourcesManager.Instance.SpendBones(1);
			yield return new WaitForSeconds(1f);
		}
	}

	private IEnumerator HandleKayceeKerfuffleChallenge()
	{
		var playerCardsWithAttacks = GrimoraModKayceeBossSequencer.GetValidCardsForFreezing();
		if (playerCardsWithAttacks.Any())
		{
			yield return TextDisplayer.Instance.ShowUntilInput("Kaycee says it's time to freeze!");
			foreach (var card in playerCardsWithAttacks)
			{
				var modInfo = GrimoraModKayceeBossSequencer.CreateModForFreeze(card);
				if (GrimoraModKayceeBossSequencer.AbilitiesThatShouldBeRemovedWhenFrozen.Exists(card.HasAbility))
				{
					card.RemoveAbilityFromThisCard(modInfo);
				}
				else
				{
					card.AddTemporaryMod(modInfo);
				}

				card.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.1f);
				card.OnStatsChanged();
			}
		}
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return deathSlot.IsPlayerSlot;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Log.LogDebug(
			$"[GModBattleSequencer] Adding [{card.InfoName()}] to cardsThatHaveDiedThisGame. "
		+ $"Current count [{_cardsThatHaveDiedThisMatch.Count + 1}]"
		);
		_cardsThatHaveDiedThisMatch.Add(card.Info);
		yield break;
	}

	public override List<CardInfo> GetFixedOpeningHand()
	{
		Log.LogDebug($"[GetFixedOpeningHand] Getting randomized list for starting hand");
		var cardsToAdd = new List<CardInfo>();
		var gravedigger = GrimoraSaveUtil.DeckList.Find(info => info.name.Equals(NameGravedigger));
		var bonePile = GrimoraSaveUtil.DeckList.Find(info => info.name.Equals(NameBonepile));
		if (bonePile)
		{
			cardsToAdd.Add(bonePile);
		}
		else if (gravedigger)
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
			if (!ActiveEnemyPiece.SafeIsUnityNull())
			{
				ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(ActiveEnemyPiece.name);
			}

			_cardsThatHaveDiedThisMatch.Clear();
			GrimoraItemsManagerExt.Instance.SetHammerActive();
		}

		foreach (var slot in BoardManager.Instance.AllSlotsCopy)
		{
			var nonCardReceivers = slot.GetComponentsInChildren<NonCardTriggerReceiver>();
			foreach (var nonCardTriggerReceiver in nonCardReceivers)
			{
				Log.LogWarning($"[GameEnd] Destroying NonCardTriggerReceiver [{nonCardTriggerReceiver}] from slot [{slot}]");
				if (nonCardTriggerReceiver is SkinCrawlerSlot crawlerSlot && crawlerSlot.skinCrawlerCard)
				{
					Log.LogWarning($"[GameEnd] Playing exit for playable card as it exists but not on the board technically [{crawlerSlot.skinCrawlerCard}] from slot [{slot}]");
					crawlerSlot.skinCrawlerCard.ExitBoard(0.3f, Vector3.zero);
					yield return new WaitForSeconds(0.2f);
				}

				Destroy(nonCardTriggerReceiver.gameObject);
			}
		}
	}

	public static IEnumerator GlitchOutBoardAndHandCards()
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
		try
		{
			(c.Anim as GravestoneCardAnimationController)?.PlayGlitchOutAnimation();
		}
		catch (Exception e)
		{
			Log.LogError($"Was unable to play glitch out for card [{c.InfoName()}], just destroying it instead.");
		}

		Destroy(c.gameObject, 0.25f);
	}
}
