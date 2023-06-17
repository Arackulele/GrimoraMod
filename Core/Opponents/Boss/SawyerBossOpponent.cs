using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using UnityEngine.TextCore;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SawyerBossOpponent : BaseBossExt
{
	public static readonly OpponentManager.FullOpponent FullOpponent = OpponentManager.Add(
		GUID,
		"SawyerBoss",
		GrimoraModSawyerBossSequencer.FullSequencer.Id,
		typeof(SawyerBossOpponent)
	);

	public override StoryEvent EventForDefeat => GrimoraEnums.StoryEvents.SawyerDefeated;

	public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{
			NumLives = 3;
			GrimoraModSawyerBossSequencer.sawyerbank = 2;
		}
		else NumLives = 2;

		PlayTheme();

		encounter.startConditions = new List<EncounterData.StartCondition>()
		{
			new()
			{
				cardsInOpponentSlots = new[] { null, NameKennel.GetCardInfo() }
			}
		};

		SpawnScenery("CratesTableEffects");
		yield return new WaitForSeconds(0.1f);

		ViewManager.Instance.SwitchToView(View.Default);

		SetSceneEffectsShownSawyer();

		yield return base.IntroSequence(encounter);
		yield return new WaitForSeconds(0.1f);

		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"LOOK AWAY, LOOK AWAY! IF YOU WANT TO FIGHT, GET IT OVER QUICK!",
			-0.65f,
			0.4f
		);

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{

			yield return TextDisplayer.Instance.ShowUntilInput($"oh... I- I see you're here again... Please, get rid of that monster and maybe I'll find some more time for you");

			bossSkull.EnterHand();
		}

		ViewManager.Instance.SwitchToView(View.Default);
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing sawyer theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Sawyer_Dogbite_Phase1", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.9f, 3f, 1);
	}

	private static void SetSceneEffectsShownSawyer()
	{
		Color orange = GameColors.Instance.orange;
		Color yellow = GameColors.Instance.yellow;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.darkGold,
			orange,
			yellow,
			yellow,
			orange,
			yellow,
			GameColors.Instance.brown,
			orange,
			GameColors.Instance.brown
		);
	}

	public static CardInfo GetRandomCard(int maxbones)
	{
		List<CardInfo> playablecards = new List<CardInfo>(GrimoraPlugin.AllPlayableGrimoraModCards);
		List<CardInfo> validcards = new List<CardInfo>();
		if (maxbones > 0)
		{
			Log.LogDebug($"2");
			foreach (var i in playablecards)
			{
				if (i.BonesCost > maxbones | i.BonesCost == 0 | i.EnergyCost > 0 | i.BonesCost > 7) { }
				else validcards.Add(i);
			}

			return validcards.GetRandomItem();
		}
		else return null;

	}

	public static CardInfo GetRandomCardSoul(int maxsoul)
	{
		List<CardInfo> playablecards = new List<CardInfo>(GrimoraPlugin.AllPlayableGrimoraModCards);
		List<CardInfo> validcards = new List<CardInfo>();
		if (maxsoul > 0)
		{
			Log.LogDebug($"2");
			foreach (var i in playablecards)
			{
				if (i.EnergyCost > maxsoul | i.EnergyCost == 0 | i.BonesCost > 0 ){ }
				else validcards.Add(i);
			}

			return validcards.GetRandomItem();
		}
		else return null;

	}

	public override IEnumerator StartNewPhaseSequence()
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && NumLives == 1)
		{
			AudioController.Instance.StopAllLoops();
			AudioController.Instance.SetLoopAndPlay("Sawyer_Dogbite_Phase1", 1);
			AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
			AudioController.Instance.SetLoopVolume(0.9f, 3f, 1);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput($"OH, HE IS GONE... WHAT A RELIEF.");
			yield return TextDisplayer.Instance.ShowUntilInput($"THANKS FOR YOUR BONES, KIND SIR!");
			yield return TextDisplayer.Instance.ShowUntilInput($"NOW THAT THAT MONSTER IS GONE, WHY DONT WE PLAY A LITTLE LONGER");

			TableVisualEffectsManager.Instance.ChangeTableColors(
				GameColors.instance.brightLimeGreen,
				GameColors.instance.brightLimeGreen,
				GameColors.instance.brightLimeGreen,
				GameColors.instance.darkLimeGreen,
				GameColors.instance.limeGreen,
				GameColors.Instance.brightLimeGreen,
				GameColors.instance.darkLimeGreen,
				GameColors.instance.limeGreen,
			GameColors.instance.brightLimeGreen
		);


			yield return ClearQueue();

			yield return ClearBoard();

			List<CardSlot> slots = new(Singleton<BoardManager>.Instance.playerSlots);
			List<CardSlot> full = new List<CardSlot>();

			foreach (var i in slots)
			{
				if (i.Card != null)
				{
					full.Add(i);
				}
			}

			ViewManager.Instance.SwitchToView(View.Board);

			yield return TextDisplayer.Instance.ShowUntilInput($"I HOPE YOU DONT MIND, THESE ARE EXTRA CHEAP.");

			foreach (var i in full)
			{
				Log.LogDebug($"Adding card to slot, bank is: " + GrimoraModSawyerBossSequencer.sawyerbank);
				if (GrimoraModSawyerBossSequencer.sawyerbank > 0)
				{
					CardInfo selectedCard = GetRandomCard(GrimoraModSawyerBossSequencer.sawyerbank);
					Log.LogDebug($"Card Selected: "+selectedCard.name);
					if (selectedCard != null)
					{
						
						yield return i.opposingSlot.CreateCardInSlot(selectedCard);
						GrimoraModSawyerBossSequencer.sawyerbank -= (int)(selectedCard.bonesCost/2);
					}
				}

			}
		}
		else { 

			AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Sawyer_Hellhound_Phase2", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0.1f, 1);
		AudioController.Instance.FadeInLoop(7f, 0.7f, 1);
		
		yield return ClearQueue();
		yield return ClearBoard();

		InstantiateBossBehaviour<SawyerBehaviour>();
		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			$"OH NO, HE HAS ARRIVED! HE IS THIRSTY FOR YOUR {"BONES!".Red()} "
		);

		ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
		yield return BoardManager.Instance
		 .OpponentSlotsCopy[2]
		 .CreateCardInSlot(NameHellHound.GetCardInfo(), 1.0f);
		yield return new WaitForSeconds(0.8f);

		yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());

		ViewManager.Instance.SwitchToView(View.BoneTokens, lockAfter: false);
		yield return new WaitForSeconds(0.4f);
		yield return ResourcesManager.Instance.AddBones(1);
		yield return new WaitForSeconds(0.4f);

		ViewManager.Instance.SetViewUnlocked();

		yield return CardSpawner.Instance.SpawnCardToHand(NameBonehound.GetCardInfo());
		yield return CardSpawner.Instance.SpawnCardToHand(NameZombie.GetCardInfo());
		yield return CardSpawner.Instance.SpawnCardToHand(NameZombie.GetCardInfo());

		}
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Kennel },
			new(),
			new() { bp_Kennel },
			new(),
			new() { bp_Zombie, bp_Skeleton },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Skeleton },
			new(),
			new(),
			new() { bp_Bonehound },
		};

		return blueprint;
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("THANKS FOR GETTING IT OVER WITH, AND DON'T EVER RETURN!");

			yield return new WaitForSeconds(0.5f);
			yield return base.OutroSequence(true);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput("THE NEXT AREA WON'T BE SO EASY.");
			yield return TextDisplayer.Instance.ShowUntilInput("I ASKED ROYAL TO DO HIS BEST AT MAKING IT IMPOSSIBLE.");
		}
		else
		{
			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.InfinitLives))
			{

				yield return TextDisplayer.Instance.ShowUntilInput("Oh no... does this mean you will try again?");

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
