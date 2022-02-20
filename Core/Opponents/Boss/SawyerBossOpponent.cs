using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;
using static GrimoraMod.GrimoraModSawyerBossSequencer;

namespace GrimoraMod;

public class SawyerBossOpponent : BaseBossExt
{
	public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;

	public override Type Opponent => SawyerOpponent;

	public override string SpecialEncounterId => "SawyerBoss";

	public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

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
			"Look away, Look away! If you want to fight, get it over quick!",
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
		InstantiateBossBehaviour<SawyerBehaviour>();
		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"PLEASE, HE HAS ARRIVED! [c:R]RUN[c:]",
			-0.65f,
			0.4f
		);
		yield return ClearQueue();
		yield return ClearBoard();
		yield return BoardManager.Instance.CreateCardInSlot(
			NameHellHound.GetCardInfo(),
			BoardManager.Instance.OpponentSlotsCopy[2],
			1.0f
		);
		yield return new WaitForSeconds(0.4f);

		yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		ViewManager.Instance.SwitchToView(View.BoneTokens);
		yield return ResourcesManager.Instance.AddBones(2);
		yield return new WaitForSeconds(0.4f);
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Zombie },
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
				"Thanks for getting it over with, and don't ever return!",
				-0.65f,
				0.4f
			);

			yield return new WaitForSeconds(0.5f);
			yield return base.OutroSequence(true);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"The next area won't be so easy.",
				-0.65f,
				0.4f
			);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I asked Royal to do his best at making it impossible.",
				-0.65f,
				0.4f
			);
		}
		else
		{
			Log.LogDebug($"[{GetType()}] Defeated player dialogue");
			yield return TextDisplayer.Instance.ShowUntilInput(
				DefeatedPlayerDialogue,
				-0.65f,
				0.4f
			);
		}
	}
}
