using System.Collections;
using DiskCardGame;
using EasyFeedback.APIs;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
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
		foreach (var blueprint in encounter.Blueprint.turns.SelectMany(cardBlueprints => cardBlueprints))
		{
			blueprint.card.Mods.Add(new CardModificationInfo(SeaLegs.ability));
		}

		Log.LogDebug($"Assigning controller to game table");
		GameObject.Find("GameTable")
			.AddComponent<Animator>()
			.runtimeAnimatorController = AssetUtils.GetPrefab<RuntimeAnimatorController>("GrimoraGameTable");

		yield return base.IntroSequence(encounter);

		GrimoraAnimationController.Instance.SetHeadBool("face_happy", true);

		yield return ShowBossSkull();

		SetSceneEffectsShownRoyal();

		if (!ConfigHelper.Instance.isDevModeEnabled)
		{
			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput("VAR, I SEE YOU MADE IT TO ME SHIP CHALLENGER!");
			yield return TextDisplayer.Instance.ShowUntilInput("I'VE BEEN WAITING FOR A WORTHY FIGHT!");

			cannons = Instantiate(
				ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/CannonTableEffects"),
				new Vector3(1.01f, 0, 0),
				Quaternion.identity,
				BoardManager3D.Instance.transform
			);

			AudioController.Instance.PlaySound2D("boss_royal", volume: 0.5f);
			yield return new WaitForSeconds(0.1f);
			ViewManager.Instance.SwitchToView(View.Default);
		}

		PlayTheme();
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing royal theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Royal_Ruckus", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.8f, 5f, 1);
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
		TurnPlan.Clear();
		yield return ClearQueue();

		var newBlueprint = BuildNewPhaseBlueprint();
		foreach (var blueprint in newBlueprint.turns.SelectMany(cardBlueprints => cardBlueprints))
		{
			blueprint.card.Mods.Add(new CardModificationInfo(SeaLegs.ability));
		}


		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"YARRG, TWAS JUST DA FIRST ROUND!\nLETS SEE HOW YE FARE 'GAINST ME PERSONAL SHIP AN CREW!",
			-0.65f,
			0.4f
		);
		ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
		yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		yield return BoardManager.Instance.CreateCardInSlot(
			NameGhostShipRoyal.GetCardInfo(),
			BoardManager.Instance.GetOpponentOpenSlots().GetRandomItem()
		);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_FirstMateSnag },
			new(),
			new() { bp_Skeleton, bp_CaptainYellowbeard, bp_Skeleton },
			new(),
			new() { bp_CompoundFracture, bp_Skeleton },
			new(),
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Skeleton, bp_Skeleton },
			new(),
		};

		return blueprint;
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
				"HELLO AGAIN!\nI AM EXCITED FOR YOU TO SEE THIS LAST ONE.",
				-0.65f,
				0.4f
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
