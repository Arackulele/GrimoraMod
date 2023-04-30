using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using InscryptionAPI.Boons;
using InscryptionAPI.Encounters;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using static UnityEngine.UIElements.UIRAtlasManager;

namespace GrimoraMod;

public class GrimoraModBattleSequencer : SpecialBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModBattleSequencer),
		typeof(GrimoraModBattleSequencer)
	);

	public static ChessboardEnemyPiece ActiveEnemyPiece;

	private static readonly List<CardInfo> _cardsThatHaveDiedThisMatch = new List<CardInfo>();

	public static readonly List<EncounterData.StartCondition> GlobalStartCon = new List<EncounterData.StartCondition>()
			{

			new ()
			{
				cardsInOpponentSlots = new[] { null, NameObelisk.GetCardInfo(), null, null },
				cardsInPlayerSlots = new[] { null, NameDeadTree.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, NameObol.GetCardInfo(), null, null },
				cardsInPlayerSlots = new[] { null, null, null, NameDeadTree.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { NameDeadTree.GetCardInfo(), NameDisturbedGrave.GetCardInfo(), null, null },
			},

			new ()
			{
				cardsInPlayerSlots = new[] { null, NameDisturbedGrave.GetCardInfo(), null, NameDeadTree.GetCardInfo() },
			},

			new ()
			{
				cardsInPlayerSlots = new[] { null, NameDisturbedGrave.GetCardInfo(), null, NameObelisk.GetCardInfo() },
			},

			new ()
			{
				cardsInOpponentSlots = new[] { null, null , NameDeadTree.GetCardInfo(), null },
				cardsInPlayerSlots = new[] { NameVoodooDoll.GetCardInfo() , null, null, null },
			},

			new ()
			{
				cardsInOpponentSlots = new[] { null, NameVoodooDoll.GetCardInfo(), null },
				cardsInPlayerSlots = new[] { null, null, null, NameObelisk.GetCardInfo() },
			},

			};

	public static readonly List<EncounterData.StartCondition> KayceeStarts = new List<EncounterData.StartCondition>()
			{
			new ()
			{
				cardsInOpponentSlots = new[] { NameDraugr.GetCardInfo(), null, null, null },
				cardsInPlayerSlots = new[] { null , null, null, NameDraugr.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { NameGlacier.GetCardInfo(), null, null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, null, null, NameDraugr.GetCardInfo() },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameDraugr.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, NameGlacier.GetCardInfo(), null, null },
				cardsInPlayerSlots = new[] { null, NameGlacier.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameObelisk.GetCardInfo(), null, NameDraugr.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, null, NameVoodooDoll.GetCardInfo(), null },
				cardsInPlayerSlots = new[] { null , null, null, NameDraugr.GetCardInfo() },
			},

			};

	public static readonly List<EncounterData.StartCondition> SawyerStarts = new List<EncounterData.StartCondition>()
			{
			new ()
			{
				cardsInOpponentSlots = new[] { NameObol.GetCardInfo(), null, null, null },
				cardsInPlayerSlots = new[] { null , null, null, NameObelisk.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { NameKennel.GetCardInfo(), null, null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, null, null, NameObol.GetCardInfo() },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameKennel.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameObol.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameObelisk.GetCardInfo(), null, NameKennel.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, null, NameObelisk.GetCardInfo(), null },
				cardsInPlayerSlots = new[] { null , null, null, NameObol.GetCardInfo() },
			},

			};

	public static readonly List<EncounterData.StartCondition> RoyalStarts = new List<EncounterData.StartCondition>()
			{
			new ()
			{
				cardsInOpponentSlots = new[] { NameShipwreck.GetCardInfo(), null, null, null },
				cardsInPlayerSlots = new[] { null , null, null, NameShipwreck.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { NameShipwreck.GetCardInfo(), null, null, null },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, null, null, NameShipwreckDams.GetCardInfo() },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameShipwreck.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameShipwreckDams.GetCardInfo(), null, null },
			},
			new ()
			{
				cardsInPlayerSlots = new[] { null, NameObelisk.GetCardInfo(), null, NameShipwreck.GetCardInfo() },
			},
			new ()
			{
				cardsInOpponentSlots = new[] { null, null, NameShipwreckDams.GetCardInfo(), null },
				cardsInPlayerSlots = new[] { null , null, null, NameShipwreck.GetCardInfo() },
			},

			};

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{


		EncounterData data = new EncounterData
		{

			startConditions = EnemyManagerPatches.OneCondition,

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
		if (!TurnManager.Instance.PlayerIsWinner() && !AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.InfinitLives))
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

			if (SaveFile.IsAscension)
			{

				//for some reason i cant instanciate the loss Screen???

				AscensionMenuScreens.ReturningFromSuccessfulRun = false;
				AscensionMenuScreens.ReturningFromFailedRun = true;

				AscensionStatsData.TryIncrementStat(AscensionStat.Type.Losses);

			SaveManager.SaveToFile();

				//AscensionSaveData.Data.activeChallenges = new List<AscensionChallenge> { ChallengeManagement.FrailHammer };


				SceneLoader.Load("Ascension_Configure");

				//yield break;
			}
			else
			{
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

				GlitchOutAssetEffect.GlitchModel(GrimoraItemsManagerExt.Instance.HammerSlot.transform);
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

				HammerItemExt.useCounter = 0;

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
					case 4:
					{
						yield return TextDisplayer.Instance.ShowUntilInput("I hope you're able to warm up next turn.");
						break;
					}
					case 5:
						ChallengeActivationUI.TryShowActivation(ChallengeManagement.KayceesKerfuffle);
						yield return HandleKayceeKerfuffleChallenge();
						break;
				}
			}
		}
	}

		public override IEnumerator PlayerCombatEnd()
		{
		while (GameObject.Find("Weight(Clone)") != null)
				{
			GlitchOutAssetEffect.GlitchModel(GameObject.Find("Weight(Clone)").transform);
			yield return new WaitForSeconds(0.2f);
				}

		}

		public override IEnumerator OpponentCombatEnd()
	{
		while (GameObject.Find("Weight(Clone)") != null)
		{
			GlitchOutAssetEffect.GlitchModel(GameObject.Find("Weight(Clone)").transform);
			yield return new WaitForSeconds(0.2f);
		}

	}

	private IEnumerator HandleSawyersShowdownChallenge()
	{

		ViewManager.Instance.SwitchToView(View.BoneTokens);
		yield return new WaitForSeconds(0.25f);



		if (ResourcesManager.Instance.PlayerBones > 2)
		{
			yield return ResourcesManager.Instance.SpendBones(1);
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.SawyersShowdown);
			yield return TextDisplayer.Instance.ShowUntilInput("Sawyer thanks you for your contribution!");
		}
		else
		{
			yield return ResourcesManager.Instance.AddBones(1);
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.SawyersShowdown);
			yield return TextDisplayer.Instance.ShowUntilInput("Sawyer felt bad for you.");
		}

		yield return new WaitForSeconds(1f);
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
		var gravedigger = RunState.Run.playerDeck.Cards.Find(info => info.BonesCost < 2 || info.EnergyCost < 2);
		var bonePile = RunState.Run.playerDeck.Cards.Find(info => info.name!= gravedigger.name && (info.BonesCost < 3 || info.EnergyCost < 3) );
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

		Log.LogDebug($"Triggering Game end in Boss Opponent");

		if (playerWon)
		{
			// Log.LogDebug($"[GrimoraModBattleSequencer Adding enemy to config [{ActiveEnemyPiece.name}]");
			if (!ActiveEnemyPiece.SafeIsUnityNull())
			{
				GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Add(ActiveEnemyPiece.name);

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
