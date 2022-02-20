using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SawyerBossOpponent : BaseBossExt
{
	public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;

	public override Type Opponent => SawyerOpponent;

	public override string SpecialEncounterId => "SawyerBoss";

	public override string DefeatedPlayerDialogue => "MY DOGS WILL ENJOY YOUR BONES!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
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

		PlayTheme();
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing sawyer theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Dogbite", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.FadeInLoop(0.5f, 1f, 1);
	}

	private static void SetSceneEffectsShownSawyer()
	{
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.darkGold,
			GameColors.Instance.orange,
			GameColors.Instance.yellow,
			GameColors.Instance.yellow,
			GameColors.Instance.orange,
			GameColors.Instance.yellow,
			GameColors.Instance.brown,
			GameColors.Instance.orange,
			GameColors.Instance.brown
		);
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		{
			InstantiateBossBehaviour<SawyerBehaviour>();

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"PLEASE, I DON'T WANT TO FIGHT ANYMORE! GET IT OVER WITH!",
				-0.65f,
				0.4f
			);
			yield return ClearQueue();
			yield return ClearBoard();

			yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		}
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound, bp_Bonehound },
			new(),
			new(),
			new(),
			new() { bp_Bonehound, bp_Draugr, bp_Draugr },
			new(),
			new(),
			new(),
			new() { bp_Bonehound, bp_Draugr, bp_Draugr },
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
			yield return TextDisplayer.Instance.ShowUntilInput(
				"THANKS FOR GETTING IT OVER WITH, AND DON'T EVER RETURN!",
				-0.65f,
				0.4f
			);

			yield return new WaitForSeconds(0.5f);
			yield return base.OutroSequence(true);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"THE NEXT AREA WON'T BE SO EASY.",
				-0.65f,
				0.4f
			);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I ASKED ROYAL TO DO HIS BEST AT MAKING IT IMPOSSIBLE.",
				-0.65f,
				0.4f
			);
		}
		else
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				DefeatedPlayerDialogue,
				-0.65f,
				0.4f
			);
		}
	}
}
