using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using Object = UnityEngine.Object;

namespace GrimoraMod;

public class RoyalBossOpponentExt : BaseBossExt
{
	public const string SpecialId = "RoyalBoss";

	public GameObject cannons;

	public override StoryEvent EventForDefeat => StoryEvent.Part3PurchasedHoloBrush;

	public override Type Opponent => RoyalOpponent;

	public override string DefeatedPlayerDialogue => "Arrg! Walk off the plank yee dirty Scallywag!!!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		// Log.LogDebug($"[{GetType()}] Calling base IntroSequence, this creates and sets the candle skull");
		yield return base.IntroSequence(encounter);

		GrimoraAnimationController.Instance.SetHeadBool("face_happy", val: true);
		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"RoyalBossPreIntro",
			TextDisplayer.MessageAdvanceMode.Input
		);
		AudioController.Instance.PlaySound2D("boss_royal");
		yield return new WaitForSeconds(0.1f);

		Log.LogDebug($"[{GetType()}] Setting RoyalBossSkull [{RoyalBossSkull}]");
		RoyalBossSkull.SetActive(true);

		yield return base.ShowBossSkull();

		Log.LogDebug($"[{GetType()}] Creating royal mask if not null");
		Mask = RoyalBossSkull;

		Log.LogDebug($"[{GetType()}] Transforming mask");
		Mask.transform.localPosition = new Vector3(0, 0.2f, 0);
		Mask.transform.localRotation = Quaternion.Euler(90, 325, 0);

		SetSceneEffectsShownRoyal();
		yield return new WaitForSeconds(1f);

		yield return base.FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"Var, I see you made it to me ship challenger! I've been waiting for a worthy fight!",
			-0.65f,
			0.4f,
			Emotion.Neutral,
			TextDisplayer.LetterAnimation.Jitter,
			DialogueEvent.Speaker.Single, null, true
		);


		cannons = Object.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/CannonTableEffects")
		);

		ViewManager.Instance.SwitchToView(View.Default);

		yield return new WaitForSeconds(2f);
	}

	private static void SetSceneEffectsShownRoyal()
	{
		Color brightBlue = GameColors.Instance.brightBlue;
		brightBlue.a = 0.5f;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.blue,
			GameColors.Instance.marigold,
			GameColors.Instance.brightBlue,
			brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.gray,
			GameColors.Instance.gray,
			GameColors.Instance.lightGray
		);
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		Log.LogDebug($"StartNewPhaseSequence started for RoyalBoss");
		yield return base.FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"Yee be a tough nut to crack!\nReady for Round 2?",
			-0.65f,
			0.4f,
			Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter,
			DialogueEvent.Speaker.Single, null, true
		);


		var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();

		// foreach (var slot in playerSlotsWithCards)
		// {
		// 	slot.Card.Anim.PlayDeathAnimation();
		// }

		// var blueprint = BuildInitialBlueprint();

		// this.Blueprint = blueprint;

		// List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);

		// this.ReplaceAndAppendTurnPlan(plan);

		yield return this.QueueNewCards();

		yield break;
	}


	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			yield return base.FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I overestimated me skill, good luck challenger.",
				-0.65f,
				1f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null
			);

			// taken from Opponent patches as it makes more sense to glitch the cannons out once defeated
			GrimoraAnimationController.Instance.SetHeadBool("face_disappointed", val: true);
			GrimoraAnimationController.Instance.SetHeadBool("face_happy", val: false);
			yield return new WaitForSeconds(0.5f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return cannons.GetComponent<CannonTableEffects>().GlitchOutCannons();

			yield return new WaitForSeconds(0.5f);

			yield return base.OutroSequence(true);

			yield return new WaitForSeconds(0.05f);
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			yield return new WaitForSeconds(0.05f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"Hello again! I am excited for you to see this last one. I put it together myself." +
				"\nLet's see if you can beat all odds and win!",
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true
			);
		}
		else
		{
			Log.LogDebug($"[{GetType()}] Defeated player dialogue");
			yield return TextDisplayer.Instance.ShowUntilInput(
				DefeatedPlayerDialogue,
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.Jitter,
				DialogueEvent.Speaker.Single, null, true
			);

			// Log.LogDebug($"Setting footstep sound to wood");
			// FirstPersonController.Instance.SetFootstepSound(FirstPersonController.FootstepSound.Wood);
			//
			// for (int i = 0; i < 3; i++)
			// {
			// 	Log.LogDebug($"Playing footstep");
			// 	AudioController.Instance.PlaySound3D("Footsteps_Wood", MixerGroup.TableObjectsSFX, base.gameObject.transform.position, 1f, 0f, null, null, new AudioParams.Randomization());
			//
			// 	Log.LogDebug($"Camera offset position");
			// 	float zValue = -i - 3;
			// 	ViewManager.Instance.OffsetPosition(new Vector3(0f, 0f, zValue), 1.5f);
			// 	Log.LogDebug($"Waiting until next step");
			// 	yield return new WaitForSeconds(1.5f);
			// }
		}


		yield break;
	}
}
