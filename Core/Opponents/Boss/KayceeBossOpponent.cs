using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
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

	public override string DefeatedPlayerDialogue => "YOUUUUUUUR, PAINNNFULLLLL DEAAATHHH AWAIIITTTSSS YOUUUUUUU!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
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
		GameObject.Find("SnowPhase2").GetComponent<ParticleSystem>().startLifetime = 0;
		GameObject.Find("SnowPhase1").GetComponent<ParticleSystem>().startLifetime = 0;

		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput($"{"BRRRR!".BrightBlue()} I'VE BEEN FREEZING FOR AGES!");
		yield return TextDisplayer.Instance.ShowUntilInput($"LET'S TURN UP THE {"HEAT".Red()} FOR A GOOD FIGHT!");
		GameObject.Find("SnowPhase1").GetComponent<ParticleSystem>().startLifetime = 10;

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

	public override IEnumerator StartNewPhaseSequence()
	{
		{
			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput($"I'M STILL NOT FEELING {"WARMER!".Red()}");

			GameObject.Find("SnowPhase1").SetActive(false);
			GameObject.Find("SnowPhase2").GetComponent<ParticleSystem>().startLifetime = 5;

			yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		}
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			// before the mask gets put away
			yield return TextDisplayer.Instance.ShowUntilInput($"OH COME ON DUDE, I'M STILL {"COLD!".Blue()}");
			yield return TextDisplayer.Instance.ShowUntilInput("LET'S FIGHT AGAIN SOON!");

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
			yield return TextDisplayer.Instance.ShowUntilInput(DefeatedPlayerDialogue);
		}
	}
}
