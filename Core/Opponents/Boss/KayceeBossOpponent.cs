using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class KayceeBossOpponent : BaseBossExt
{
	public static readonly OpponentManager.FullOpponent FullOpponent = OpponentManager.Add(
		GUID,
		"KayceeBoss",
		GrimoraModKayceeBossSequencer.FullSequencer.Id,
		typeof(KayceeBossOpponent)
	);

	public override StoryEvent EventForDefeat => GrimoraEnums.StoryEvents.KayceeDefeated;

	GameObject Phase2Snow;

	public override string DefeatedPlayerDialogue => "YOUUUUUUUR, PAINNNFULLLLL DEAAATHHH AWAIIITTTSSS YOUUUUUUU!";


	public static int liveamnt = 2;
	public override int StartingLives => liveamnt;


	public override IEnumerator IntroSequence(EncounterData encounter)
	{

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{
			NumLives = 3;

		}
		else NumLives = 2;


		PlayTheme();

		encounter.startConditions = new List<EncounterData.StartCondition>()
		{
			new()
			{
				cardsInOpponentSlots = new[] { NameDraugr.GetCardInfo(), NameDraugr.GetCardInfo() }
			}
		};

		SetSceneEffectsShownKaycee();

		yield return base.IntroSequence(encounter);
		Phase2Snow = GameObject.Find("SnowPhase2");
		Phase2Snow.SetActive(false);
		GameObject.Find("SnowPhase1").GetComponent<ParticleSystem>().startLifetime = 0;

		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput($"{"BRRRR!".BrightBlue()} I'VE BEEN FREEZING FOR AGES!");
		yield return TextDisplayer.Instance.ShowUntilInput($"LET'S TURN UP THE {"HEAT".Red()} FOR A GOOD FIGHT!");
		GameObject.Find("SnowPhase1").GetComponent<ParticleSystem>().startLifetime = 10;

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{

			yield return TextDisplayer.Instance.ShowUntilInput($"Oh hey, looks like I've got another shot. Hope you watched the weather forecast!");

			bossSkull.EnterHand();
		}

		ViewManager.Instance.SwitchToView(View.Default);
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing kaycee theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Frostburn", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.6f, 5f, 1);
	}

	private static void SetSceneEffectsShownKaycee()
	{
		Color brightBlue = GameColors.Instance.brightBlue;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			brightBlue,
			brightBlue,
			brightBlue,
			GameColors.Instance.darkBlue,
			brightBlue,
			GameColors.Instance.nearWhite,
			GameColors.Instance.blue,
			brightBlue,
			brightBlue
		);
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Draugr },
			new() { bp_Draugr, bp_Draugr },
			new() { bp_Draugr },
			new(),
			new() { bp_Skeleton, bp_Revenant, bp_Draugr },
			new(),
			new(),
			new() { bp_Draugr, bp_Skeleton, bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new(),
			new() { bp_Skeleton },
		};

		return blueprint;
	}

	public EncounterBlueprintData BuildNewPhaseBlueprintAgain()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_EmberSpirit },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Flameskull },
			new(),
			new(),
			new() { bp_EmberSpirit },
			new(),
			new() { bp_Vampire },
			new(),
			new(),
			new() { bp_Flameskull },
			new() { bp_Flameskull },
			new(),
			new() { bp_Skeleton },
			new() { bp_EmberSpirit },
			new(),
			new(),
			new() { bp_Flameskull },
		};

		return blueprint;
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && NumLives == 1)
		{

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput($"OH, IT SEEMS I HAVE FINALLY {"WARMED UP!".Red()}");

			Phase2Snow.SetActive(false);

			AudioController.Instance.StopAllLoops();
			AudioController.Instance.SetLoopAndPlay("Frostburn", 1);
			AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
			AudioController.Instance.SetLoopVolume(0.6f, 5f, 1);

			TableVisualEffectsManager.Instance.ChangeTableColors(
	GameColors.instance.orange,
	GameColors.instance.orange,
	GameColors.instance.orange,
	GameColors.instance.yellow,
	GameColors.instance.red,
	GameColors.Instance.brightNearWhite,
	GameColors.instance.darkRed,
	GameColors.instance.red,
	GameColors.instance.brownOrange
	);


			ViewManager.Instance.SwitchToView(View.Board);

			yield return ClearQueue();

			List<CardSlot> slots = new(Singleton<BoardManager>.Instance.allSlots);


			CardModificationInfo cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { Burning.ability }
			};

			foreach (var i in slots)
			{
				if (i.Card != null)
				{
					if (i.Card.AllAbilities().Count() > 4)
					{
						i.Card.Anim.StrongNegationEffect();
						i.Card.TakeDamage(1, null);

					}
					else
					{
						i.Card.AddTemporaryMod(cardModificationInfo);
						Burning.tryburningcard(i.Card, "RedFire");
						yield return new WaitForSeconds(0.2f);

						i.Card.OnStatsChanged();
					}

				}
			}

			yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprintAgain());

		}

		else
		{
			AudioController.Instance.FadeOutLoop(20, 1);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput($"I'M STILL NOT FEELING {"WARMER!".Red()}");

			AudioController.Instance.SetLoopAndPlay("FrostburnStorm", 1);
			AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
			AudioController.Instance.SetLoopVolume(0.6f, 5f, 1);

			GameObject.Find("SnowPhase1").SetActive(false);
			Phase2Snow.SetActive(true);

			yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprint());

			ViewManager.Instance.SwitchToView(View.Board);

			yield return ClearQueue();

			if (BoardManager.Instance.PlayerSlotsCopy[0].Card != null) yield return BoardManager.Instance.PlayerSlotsCopy[0].Card.Die(false, null);
			if (BoardManager.Instance.PlayerSlotsCopy[0].Card != null) yield return BoardManager.Instance.PlayerSlotsCopy[0].Card.Die(false, null);
			yield return new WaitForSeconds(0.3f);

			if (BoardManager.Instance.OpponentSlotsCopy[0].Card != null) yield return BoardManager.Instance.OpponentSlotsCopy[0].Card.Die(false, null);
			if (BoardManager.Instance.OpponentSlotsCopy[0].Card != null) yield return BoardManager.Instance.OpponentSlotsCopy[0].Card.Die(false, null);
			yield return new WaitForSeconds(0.2f);

			yield return BoardManager.Instance.PlayerSlotsCopy[0].CreateCardInSlot(NameAvalanche.GetCardInfo());
			yield return new WaitForSeconds(0.3f);
			yield return BoardManager.Instance.OpponentSlotsCopy[0].CreateCardInSlot(NameAvalanche.GetCardInfo());
			yield return new WaitForSeconds(0.3f);
			yield return TurnManager.Instance.Opponent.QueueCard(NameAvalanche.GetCardInfo(), BoardManager.Instance.GetQueueSlots()[0]);
			yield return new WaitForSeconds(0.3f);
			yield return TextDisplayer.Instance.ShowUntilInput($"LETS SEE HOW YOU DEAL WITH {"THIS!".BrightBlue()}");


		}
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			// before the mask gets put away
			yield return TextDisplayer.Instance.ShowUntilInput($"OH COME ON DUDE, I'M STILL {"COLD!".Blue()}");
			yield return TextDisplayer.Instance.ShowUntilInput("LET'S FIGHT AGAIN SOON!");

			Phase2Snow.SetActive(false);
			// this will put the mask away
			yield return base.OutroSequence(true);


			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"FOR DEFEATING ONE OF MY GHOULS, I WILL REWARD YOU A STARTING BONE IN EACH OF YOUR BATTLES."
			);
			yield return TextDisplayer.Instance.ShowUntilInput("THIS NEXT AREA WAS MADE BY ONE OF MY GHOULS, SAWYER.");
			yield return TextDisplayer.Instance.ShowUntilInput("HE SAYS IT IS TERRIBLE.");
		}
		else
		{
			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.InfinitLives))
			{

				yield return TextDisplayer.Instance.ShowUntilInput("C'mon dude, where are you going? Can't handle the cold?");
				Phase2Snow.SetActive(false);

				AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX);

				yield return HideRightHandBossSkull();

				DestroyScenery();

				SetSceneEffectsShown(false);

				AudioController.Instance.StopAllLoops();

				yield return new WaitForSeconds(0.75f);

				CleanUpBossBehaviours();

				ViewManager.Instance.SwitchToView(View.Default, false, true);

				TableVisualEffectsManager.Instance.ResetTableColors();
				yield return new WaitForSeconds(0.25f);
			}
			else yield return TextDisplayer.Instance.ShowUntilInput(DefeatedPlayerDialogue);
		}


		}
	}
