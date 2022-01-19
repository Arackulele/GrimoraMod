using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod;

public class SawyerBossOpponent : BaseBossExt
{
	public const string SpecialId = "SawyerBoss";

	public override StoryEvent EventForDefeat => StoryEvent.FactoryCuckooClockAppeared;

	public override Type Opponent => SawyerOpponent;

	public override string DefeatedPlayerDialogue => "My dogs will enjoy your bones!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead");
		AudioController.Instance.SetLoopAndPlay("gbc_battle_undead", 1);
		base.SpawnScenery("CratesTableEffects");
		yield return new WaitForSeconds(0.5f);


		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);

		SetSceneEffectsShownSawyer();

		yield return base.IntroSequence(encounter);
		yield return new WaitForSeconds(0.5f);

		yield return base.FaceZoomSequence();
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"Look away, Look away! If you want to fight, get it over quick!", -0.65f, 0.4f, Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);

		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
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

	public override EncounterBlueprintData BuildInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { },
			new() { bp_Skeleton, bp_BoneSerpent },
			new() { },
			new() { },
			new() { bp_Sarcophagus, bp_BoneSerpent },
			new() { },
			new() { bp_Skeleton, bp_BoneSerpent },
			new() { },
			new() { bp_BoneSerpent },
			new() { },
			new() { bp_UndeadWolf },
			new() { bp_BoneSerpent, bp_BoneSerpent }
		};

		return blueprint;
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		{
			base.InstantiateBossBehaviour<SawyerBehaviour>();

			yield return base.FaceZoomSequence();
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
				"Please, I don't want to fight anymore! Get it over with!", -0.65f, 0.4f, Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);


			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() { bp_Skeleton },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_UndeadWolf },
				new() { },
				new() { bp_BoneSerpent },
				new() { bp_BoneSerpent },
				new() { },
				new() { bp_UndeadWolf },
				new() { bp_BoneSerpent, bp_BoneSerpent }
			};

			yield return ReplaceBlueprintCustom(blueprint);
		}
		yield break;
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"Thanks for getting it over with, and don't ever return!",
			-0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
		TableVisualEffectsManager.Instance.ResetTableColors();

		yield return new WaitForSeconds(1f);


		yield return base.FaceZoomSequence();
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
			"The next area won't be so easy. I asked Royal to do his best at making it impossible.", -0.65f, 0.4f,
			Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);


		yield break;
	}
}
