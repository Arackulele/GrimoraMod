using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GrimoraGameFlowManager), "SceneSpecificInitialization")]
	public class GrimoraGameFlowManagerPatches
	{
		[HarmonyPrefix]
		public static bool Prefix(GrimoraGameFlowManager __instance)
		{
			if (SaveManager.SaveFile.IsGrimora)
			{
				bool skipIntro = false;
				bool skipTombstone = true;
				if (FinaleDeletionWindowManager.instance != null)
				{
					Object.Destroy(Object.FindObjectOfType<FinaleDeletionWindowManager>().gameObject);
				}
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, immediate: true);
				
				if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
				{
					
					StoryEventsData.SetEventCompleted(StoryEvent.PlayerDeletedArchivistFile, true);
					StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
					StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
					StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
					SaveManager.SaveToFile();
					
					if (Singleton<GameMap>.Instance != null)
					{
						Singleton<GameMap>.Instance.HideMapImmediate();
					}
					__instance.CurrentGameState = GameState.FirstPerson3D;
					__instance.StartCoroutine(__instance.TransitionTo(GameState.FirstPerson3D, null, immediate: true));
					Singleton<ExplorableAreaManager>.Instance.HangingLight.gameObject.SetActive(skipTombstone);
					Singleton<ExplorableAreaManager>.Instance.HandLight.gameObject.SetActive(skipTombstone);
					__instance.gameTableCandlesParent.SetActive(skipTombstone);
					__instance.gravestoneNavZone.SetActive(skipTombstone);
					if (!skipIntro)
					{
						__instance.StartCoroutine(__instance.StartSceneSequence());
					}
				}
				else
				{
					AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
					if (Singleton<GameMap>.Instance != null)
					{
						__instance.CurrentGameState = GameState.Map;
						__instance.StartCoroutine(__instance.TransitionTo(GameState.Map, null, immediate: true));
					}
				}
				
				// SaveManager.saveFile.grimoraData.removedPieces = new List<int>();
				
				// SaveManager.SaveToFile();

				#region CardRemover
				
				//just to make a fucking select slot for old custom nodes
				var cardremover =
					(GameObject)Object.Instantiate(Resources.Load("prefabs\\specialnodesequences\\CardRemoveSequencer"));
				cardremover.gameObject.transform.position = Vector3.negativeInfinity;
				
				var rarechoicesgenerator =
					Object.Instantiate(Object.FindObjectOfType<GrimoraCardChoiceGenerator>().gameObject);
				rarechoicesgenerator.name = "RareChoices";
				Object.Destroy(rarechoicesgenerator.GetComponent<CardSingleChoicesSequencer>());
				rarechoicesgenerator.AddComponent<RareCardChoicesSequencer>();
				rarechoicesgenerator.transform.parent =
					Object.FindObjectOfType<SpecialNodeHandler>().gameObject.transform;
				Object.FindObjectOfType<SpecialNodeHandler>().rareCardChoiceSequencer =
					rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>();
				rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().deckPile =
					rarechoicesgenerator.GetComponentInChildren<CardPile>();
				var rarebox = (GameObject)Object.Instantiate(Resources.Load("prefabs\\specialnodesequences\\RareCardBox"));
				rarebox.transform.parent = rarechoicesgenerator.transform;
				rarebox.gameObject.transform.localPosition = new Vector3(-0.1f, rarebox.gameObject.transform.position.y, 99);
				rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().box = rarebox.transform;
				rarechoicesgenerator.AddComponent<Part1RareChoiceGenerator>();
				rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().choiceGenerator =
					rarechoicesgenerator.GetComponent<Part1RareChoiceGenerator>();
				rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().selectableCardPrefab =
					(GameObject)Resources.Load("prefabs\\cards\\SelectableCard_Grimora");
				rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().gamepadGrid =
					rarechoicesgenerator.GetComponent<GamepadGridControl>();

				#endregion
				
				return false;
			}

			return true;
		}
	}
}