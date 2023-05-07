using System.Collections;
using System.Runtime.InteropServices;
using DiskCardGame;
using InscryptionAPI.Boons;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class RoyalBossOpponentExt : BaseBossExt
{
	public static readonly OpponentManager.FullOpponent FullOpponent = OpponentManager.Add(
		GUID,
		"RoyalBoss",
		GrimoraModRoyalBossSequencer.FullSequencer.Id,
		typeof(RoyalBossOpponentExt)
	);

	public GameObject cannons;

	public override StoryEvent EventForDefeat => GrimoraEnums.StoryEvents.RoyalDefeated;

	public override string DefeatedPlayerDialogue => "ARRG! WALK OFF THE PLANK YEE DIRTY SCALLYWAG!!!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{
			NumLives = 3;
		}
		else NumLives = 2;

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones) && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.Soulless))
		{
			encounter.startConditions = new List<EncounterData.StartCondition>()
				{
					new()
					{
						cardsInOpponentSlots = new[] { NameShipwreck.GetCardInfo(), null, null, null }
					}
				};
		}
		else
		{

			encounter.startConditions = new List<EncounterData.StartCondition>()
		{
			new()
			{
				cardsInOpponentSlots = new[] { NameShipwreck.GetCardInfo(), null, null, NameRevenant.GetCardInfo() }
			}
		};

	}

		yield return base.IntroSequence(encounter);

		GrimoraAnimationController.Instance.SetHeadBool("face_happy", true);

		SetSceneEffectsShownRoyal();

		if (!ConfigHelper.Instance.IsDevModeEnabled)
		{
			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput("VAR, I SEE YOU MADE IT TO ME SHIP CHALLENGER!");
			yield return TextDisplayer.Instance.ShowUntilInput("I'VE BEEN WAITING FOR A WORTHY FIGHT!");
		}


		cannons = Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/CannonTableEffects"),
			new Vector3(1.01f, 0, 0),
			Quaternion.identity,
			BoardManager3D.Instance.transform
		);

		if (!ConfigHelper.Instance.IsDevModeEnabled)
		{
			AudioController.Instance.PlaySound2D("boss_royal", volume: 0.5f);
			yield return new WaitForSeconds(0.1f);
		}

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{

			yield return TextDisplayer.Instance.ShowUntilInput($"IT SEEMS A STORM IS BREWING, I HOPE ME SHIP CAN HANDLE IT");

			bossSkull.EnterHand();
		}

		ViewManager.Instance.SwitchToView(View.Default);

		PlayTheme();


		GameObject.Find("BoardLight").GetComponent<Light>().cookie = ResourceBank.Get<Texture>("Art/Effects/WavesTextureCube");
		GameObject.Find("BoardLight_Cards").GetComponent<Light>().cookie = ResourceBank.Get<Texture>("Art/Effects/WavesTextureCube");
		Tween.Rotate(Singleton<ExplorableAreaManager>.Instance.HangingLight.transform, new Vector3(150f, 0f, 0f), Space.World, 25f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);


		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones) && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.Soulless))
		{
			yield return BoardManager.Instance.opponentSlots[3].CreateCardInSlot(NameRevenant.GetCardInfo(), 1.0f);
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.NoBones);
			yield return TextDisplayer.Instance.ShowUntilInput("OH, I SEE YEE ARE SEVERELY HAUNTED!");
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.Soulless);
			yield return TextDisplayer.Instance.ShowUntilInput("REVENANT, PACK YE BAGS, YOURE GETTING THROWN OVERBOARD!");
			yield return BoardManager.Instance.opponentSlots[3].Card.Die(false, null);
		}
	}

	public override void ModifyQueuedCard(PlayableCard card)
	{
		AddAnchoredAbility(card);
	}

	public override void ModifySpawnedCard(PlayableCard card)
	{
		AddAnchoredAbility(card);
	}

	private void AddAnchoredAbility(PlayableCard playableCard)
	{
		if (!playableCard.TemporaryMods.Exists(mod => mod.abilities.Contains(Anchored.ability)))
		{
			playableCard.AddTemporaryMod(new CardModificationInfo(Anchored.ability));
		}
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing royal theme 1");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("RoyalRuckus_Phase1", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.8f, 5f, 1);
	}

	private static void SetSceneEffectsShownRoyal()
	{
		Color brightBlue = GameColors.Instance.brightBlue;
		Color brightBlueModified = GameColors.Instance.brightBlue;
		brightBlueModified.a = 0.5f;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.blue,
			GameColors.Instance.marigold,
			brightBlue,
			brightBlueModified,
			brightBlue,
			brightBlue,
			GameColors.Instance.gray,
			GameColors.Instance.gray,
			GameColors.Instance.lightGray
		);
	}
	GameObject Rain;
	AudioSource oceanSound;
	public override IEnumerator StartNewPhaseSequence()
	{

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && NumLives == 1)
		{

			yield return TextDisplayer.Instance.ShowUntilInput(
"ME SHIP! I FEAR WE MAY BE SINKING."
);

			yield return ClearQueue();

			yield return ClearBoard();

			if (GameObject.Find("RainParticles(Clone)") != null) Destroy(GameObject.Find("RainParticles(Clone)"));
			if (Rain != null) Destroy(Rain);

			oceanSound = AudioController.Instance.PlaySound2D("ocean_fall");

			yield return new WaitForSeconds(4);

			TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.instance.nearBlack,
			GameColors.instance.darkBlue,
			GameColors.instance.nearBlack,
			GameColors.instance.blue,
			GameColors.instance.blue,
			GameColors.Instance.brightBlue,
			GameColors.instance.blue,
			GameColors.instance.blue,
			GameColors.instance.brightBlue
		);


			if (ConfigHelper.Instance.DisableMotionSicknessEffects == false) Singleton<CameraEffects>.Instance.ShowUnderwater();



			yield return TextDisplayer.Instance.ShowUntilInput(
				"PREPARE TO HOLD YEE BREATH!",
				-0.65f,
				0.4f
			);

			yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprintAgain());

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

			yield return TextDisplayer.Instance.ShowUntilInput($"HOLY FISHPASTE, YE FOUND DAVY JONES TREASURE");

			foreach (var i in full)
			{

				yield return i.opposingSlot.CreateCardInSlot(NameDavyJonesLocker.GetCardInfo());

			}
		}
		else
		{
			AudioController.Instance.FadeOutLoop(5, 1);

			TurnPlan.Clear();
			yield return ClearQueue();
			yield return ClearBoard();

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"YARRG, TWAS JUST DA FIRST ROUND!\nLETS SEE HOW YE FARE 'GAINST ME PERSONAL SHIP AN CREW!",
				-0.65f,
				0.4f
			);
			ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
			Rain = GameObject.Instantiate(GameObject.Instantiate(kopieGameObjects.Find(g => g.name.Contains("RainParticles"))));
			Rain.transform.parent = GameObject.Find("RoyalBossSkull(Clone)").transform;

			yield return ReplaceBlueprintCustom(BuildNewPhaseBlueprint());

			yield return BoardManager.Instance
			 .GetOpponentOpenSlots()
			 .GetRandomItem()
			 .CreateCardInSlot(NamePirateFirstMateSnag.GetCardInfo());

			yield return new WaitForSeconds(0.25f);

			yield return BoardManager.Instance
			 .GetOpponentOpenSlots()
			 .GetRandomItem()
			 .CreateCardInSlot(NameGhostShipRoyal.GetCardInfo());

			ViewManager.Instance.SetViewUnlocked();

			Log.LogDebug($"Playing royal theme 2");
			AudioController.Instance.SetLoopAndPlay("RoyalRuckus_Phase2Rain", 1);
			AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
			AudioController.Instance.SetLoopVolume(0.8f, 5f, 1);

		}
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Shipwreck },
			new(),
			new() { bp_CaptainYellowbeard },
			new(),
			new() { bp_Shipwreck },
			new(),
			new() { bp_Revenant ,bp_Skeleton },
			new(),
			new(),
			new() { bp_FirstMateSnag, bp_Skeleton },
			new(),
			new(),
			new() { bp_CaptainYellowbeard },
			new(),
			new() { bp_Skeleton, bp_Skeleton },
			new(),
		};
		return blueprint;
	}

	public EncounterBlueprintData BuildNewPhaseBlueprintAgain()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Privateer, bp_DavyJones },
			new(),
			new(),
			new() { bp_Nixie },
			new(),
			new() { bp_Revenant },
			new() { bp_Privateer },
			new(),
			new() { bp_FirstMateSnag },
			new(),
			new() { bp_Revenant },
			new(),
			new() { bp_Privateer },
			new(),
			new() { bp_Nixie },
			new(),
			new() { bp_DavyJones },
			new(),
			new() { bp_Privateer, bp_Privateer },
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

			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
			{
				AudioController.Instance.FadeSourceVolume(oceanSound, 0f, 5.5f);

				(Singleton<TurnManager>.Instance.SpecialSequencer as GrimoraMod.GrimoraModRoyalBossSequencer).CleanupTargetIcons();

				if (ConfigHelper.Instance.DisableMotionSicknessEffects == false)
				{	Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetAlpha(1);
				Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetIntensity(0f, 1);
				Singleton<CameraEffects>.Instance.blur.enabled = false;
				Singleton<CameraEffects>.Instance.blur.blurSize = 3.5f;
				Singleton<CameraEffects>.Instance.fisheye.enabled = false;
			}


			}

			// taken from Opponent patches as it makes more sense to glitch the cannons out once defeated
			GrimoraAnimationController.Instance.SetHeadBool("face_disappointed", true);
			GrimoraAnimationController.Instance.SetHeadBool("face_happy", false);
			yield return new WaitForSeconds(0.5f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return cannons.GetComponent<CannonTableEffects>().GlitchOutCannons();
			GlitchOutAssetEffect.GlitchModel(cannons.transform);

			yield return new WaitForSeconds(0.5f);

			if (Rain!= null) Destroy(Rain);
			if (GameObject.Find("RainParticles(Clone)") != null) Destroy(GameObject.Find("RainParticles(Clone)"));

			yield return base.OutroSequence(true);
			GameObject.Find("BoardLight").GetComponent<Light>().cookie = null;
			GameObject.Find("BoardLight_Cards").GetComponent<Light>().cookie = null;
			Tween.Cancel(Singleton<ExplorableAreaManager>.Instance.HangingLight.transform.GetInstanceID());

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
			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.InfinitLives))
			{

				yield return TextDisplayer.Instance.ShowUntilInput("I beat the livin' PULP out o' ye!");

				GameObject.Find("BoardLight").GetComponent<Light>().cookie = null;
				GameObject.Find("BoardLight_Cards").GetComponent<Light>().cookie = null;
				Tween.Cancel(Singleton<ExplorableAreaManager>.Instance.HangingLight.transform.GetInstanceID());
				yield return cannons.GetComponent<CannonTableEffects>().GlitchOutCannons();
				GlitchOutAssetEffect.GlitchModel(cannons.transform);

				if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
				{
					AudioController.Instance.FadeSourceVolume(oceanSound, 0f, 5.5f);

					(Singleton<TurnManager>.Instance.SpecialSequencer as GrimoraMod.GrimoraModRoyalBossSequencer).CleanupTargetIcons();

					if (ConfigHelper.Instance.DisableMotionSicknessEffects == false)
					{
						Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetAlpha(1);
						Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetIntensity(0f, 1);
						Singleton<CameraEffects>.Instance.blur.enabled = false;
						Singleton<CameraEffects>.Instance.blur.blurSize = 3.5f;
						Singleton<CameraEffects>.Instance.fisheye.enabled = false;
					}


				}

				if (Rain != null) Destroy(Rain);
				if (GameObject.Find("RainParticles(Clone)") != null) Destroy(GameObject.Find("RainParticles(Clone)"));

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
			else yield return TextDisplayer.Instance.ShowUntilInput(
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
	}
}
