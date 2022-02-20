using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class RoyalBossOpponentExt : BaseBossExt
{
	public GameObject cannons;

	public override StoryEvent EventForDefeat => StoryEvent.Part3PurchasedHoloBrush;

	public override Type Opponent => RoyalOpponent;

	public override string SpecialEncounterId => "RoyalBoss";

	public override string DefeatedPlayerDialogue => "ARRG! WALK OFF THE PLANK YEE DIRTY SCALLYWAG!!!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		PlayTheme();

		yield return base.IntroSequence(encounter);

		GrimoraAnimationController.Instance.SetHeadBool("face_happy", true);

		GrimoraAnimationController.Instance.bossSkull = RoyalBossSkull;
		RoyalBossSkull.SetActive(true);

		yield return ShowBossSkull();

		SetSceneEffectsShownRoyal();

		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput("VAR, I SEE YOU MADE IT TO ME SHIP CHALLENGER!");
		yield return TextDisplayer.Instance.ShowUntilInput("I'VE BEEN WAITING FOR A WORTHY FIGHT!");

		cannons = Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/CannonTableEffects")
		);
		if (!ConfigHelper.Instance.isDevModeEnabled)
		{
			AudioController.Instance.PlaySound2D("boss_royal");
			yield return new WaitForSeconds(0.1f);
		}

		ViewManager.Instance.SwitchToView(View.Default);

		yield return new WaitForSeconds(2f);
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing royal theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Royal_Ruckus");
		AudioController.Instance.SetLoopVolumeImmediate(0f);
		AudioController.Instance.FadeInLoop(5f, 0.75f);
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
		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"YEE BE A TOUGH NUT TO CRACK!\nREADY FOR ROUND 2?",
			-0.65f,
			0.4f
		);

		yield return QueueNewCards();
	}


	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			AudioController.Instance.FadeOutLoop(5f);
			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I OVERESTIMATED ME SKILL, GOOD LUCK CHALLENGER.",
				-0.65f,
				1f
			);

			// taken from Opponent patches as it makes more sense to glitch the cannons out once defeated
			GrimoraAnimationController.Instance.SetHeadBool("face_disappointed", true);
			GrimoraAnimationController.Instance.SetHeadBool("face_happy", false);
			yield return new WaitForSeconds(0.5f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return cannons.GetComponent<CannonTableEffects>().GlitchOutCannons();

			yield return new WaitForSeconds(0.5f);

			yield return base.OutroSequence(true);

			yield return new WaitForSeconds(0.05f);
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			yield return new WaitForSeconds(0.05f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"HELLO AGAIN!\nI AM EXCITED FOR YOU TO SEE THIS LAST ONE.", -0.65f, 0.4f
			);

			yield return TextDisplayer.Instance.ShowUntilInput("I PUT IT TOGETHER MYSELF.");
			yield return TextDisplayer.Instance.ShowUntilInput("LET'S SEE IF YOU CAN BEAT ALL ODDS AND WIN!");
		}
		else
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				DefeatedPlayerDialogue,
				-0.65f,
				0.4f
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
