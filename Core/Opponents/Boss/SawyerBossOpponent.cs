using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using UnityEngine;
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
		PlayTheme();

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

	public override IEnumerator StartNewPhaseSequence()
	{
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
		yield return BoardManager.Instance.CreateCardInSlot(
			NameHellHound.GetCardInfo(),
			BoardManager.Instance.OpponentSlotsCopy[2],
			1.0f
		);
		yield return new WaitForSeconds(0.8f);

		yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());

		ViewManager.Instance.SwitchToView(View.BoneTokens, lockAfter: false);
		yield return new WaitForSeconds(0.4f);
		yield return ResourcesManager.Instance.AddBones(1);
		yield return new WaitForSeconds(0.4f);

		ViewManager.Instance.SetViewUnlocked();
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Kennel, bp_Kennel },
			new(),
			new() { bp_Bonehound },
			new(),
			new() { bp_Zombie, bp_Skeleton },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Bonehound },
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
			yield return TextDisplayer.Instance.ShowUntilInput(DefeatedPlayerDialogue);
		}
	}
}
