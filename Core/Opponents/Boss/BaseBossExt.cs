using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public abstract class BaseBossExt : Part1BossOpponent
	{

		public const Opponent.Type KayceeOpponent = (Type)1001;
		public const Opponent.Type DoggyOpponent = (Type)1002;
		public const Opponent.Type RoyalOpponent = (Type)1003;
		public const Opponent.Type GrimoraOpponent = (Type)1004;
		
		public static readonly Dictionary<string, Type> BossTypesByString = new()
		{
			{ "DoggyBoss", DoggyOpponent },
			{ "GrimoraBoss", GrimoraOpponent },
			{ "KayceeBoss", KayceeOpponent },
			{ "RoyalBoss", RoyalOpponent }
		};

		public abstract StoryEvent EventForDefeat { get; }
		
		public abstract Opponent.Type Opponent { get; }

		private bool HasBeenDefeated => StoryEventsData.EventCompleted(EventForDefeat);

		public void SetDefeated()
		{
			StoryEventsData.SetEventCompleted(EventForDefeat, true);
		}

		public GameObject Mask { get; set; }

		public override IEnumerator IntroSequence(EncounterData encounter)
		{

			yield return ReplaceBlueprintCustom(BuildInitialBlueprint());
			
			AudioController.Instance.FadeOutLoop(0.75f);
			RunState.CurrentMapRegion.FadeOutAmbientAudio();
			
			yield return base.IntroSequence(encounter);
			
			AudioController.Instance.SetLoopAndPlay("boss_prospector_base");
			AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1);
			base.SpawnScenery("ForestTableEffects");
			yield return new WaitForSeconds(0.5f);
			AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f);
			yield return new WaitForSeconds(0.25f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(1.25f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.15f);
			GrimoraAnimationController.Instance.ShowBossSkull();
			GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
			Destroy(GameObject.Find("RoyalBossSkull"));
			Mask = (GameObject)Instantiate(Resources.Load(
					"Prefabs/Opponents/Leshy/Masks/MaskWoodcarver"), 
				GameObject.Find("Grimora_RightWrist").transform, 
				true
			);
			Mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
			Mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

			yield return new WaitForSeconds(1.5f);
			yield return base.FaceZoomSequence();
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input
			);
			ViewManager.Instance.SwitchToView(View.Default);
			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
		}

		public virtual IEnumerator ReplaceBlueprintCustom(EncounterBlueprintData blueprintData)
		{
			base.Blueprint = blueprintData;
			List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
			this.ReplaceAndAppendTurnPlan(plan);
			yield return QueueNewCards();
		}

		public abstract EncounterBlueprintData BuildInitialBlueprint();
	}
}