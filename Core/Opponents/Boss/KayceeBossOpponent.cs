using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class KayceeBossOpponent : BaseBossExt
{
	public override StoryEvent EventForDefeat => StoryEvent.FactoryConveyorBeltMoved;

	public override Type Opponent => KayceeOpponent;
	public override string SpecialEncounterId => "KayceeBoss";

	public override string DefeatedPlayerDialogue => "Youuuuuuur, painnnfulllll deaaathhh awaiiitttsss youuuuuuu!";

	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		encounter.startConditions = new List<EncounterData.StartCondition>()
		{
			new()
			{
				cardsInOpponentSlots = new[] { NameDraugr.GetCardInfo(), NameDraugr.GetCardInfo() }
			}
		};

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);
		SetSceneEffectsShownKaycee();

		yield return base.IntroSequence(encounter);

		yield return FaceZoomSequence();
		yield return TextDisplayer.Instance.ShowUntilInput(
			"[c:bB]Brrrr![c:] I've been freezing for ages!",
			-0.65f,
			0.4f
		);
		yield return TextDisplayer.Instance.ShowUntilInput(
			"Let's turn up the [c:R]heat[c:] for a good fight!",
			-0.65f,
			0.4f
		);

		ViewManager.Instance.SwitchToView(View.Default);
		
		PlayTheme();
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing kaycee theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Frostburn", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.FadeInLoop(0.5f, 0.5f, 1);
	}

	private static void SetSceneEffectsShownKaycee()
	{
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.darkBlue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.nearWhite,
			GameColors.Instance.blue,
			GameColors.Instance.brightBlue,
			GameColors.Instance.brightBlue
		);
	}

	public EncounterBlueprintData BuildNewPhaseBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Draugr, bp_Draugr },
			new() { bp_Draugr, bp_Draugr },
			new() { bp_Skeleton },
			new(),
			new() { bp_Skeleton, bp_Revenant, bp_Draugr },
			new(),
			new(),
			new() { bp_Draugr, bp_Skeleton, bp_Draugr, bp_Revenant },
			new() { bp_Skeleton, bp_Skeleton, },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
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
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I'm still not feeling Warmer!",
				-0.65f,
				0.4f
			);

			yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
		}
	}

	public override IEnumerator OutroSequence(bool wasDefeated)
	{
		if (wasDefeated)
		{
			// before the mask gets put away
			yield return TextDisplayer.Instance.ShowUntilInput("Oh come on dude, I'm still Cold!", -0.65f, 0.4f);
			yield return TextDisplayer.Instance.ShowUntilInput("Let's fight again soon!", -0.65f, 0.4f);

			// this will put the mask away
			yield return base.OutroSequence(true);

			yield return FaceZoomSequence();
			yield return TextDisplayer.Instance.ShowUntilInput(
				"This next area was made by one of my ghouls, Sawyer.",
				-0.65f,
				0.4f
			);
			yield return TextDisplayer.Instance.ShowUntilInput("He says it is terrible.", -0.65f, 0.4f);
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
