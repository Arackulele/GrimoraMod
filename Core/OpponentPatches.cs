using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(Opponent))]
	public class OpponentPatches
	{
		// this.AddComponent(typeof (T)) as T;
		private static readonly Dictionary<Opponent.Type, Type> BossBattleSequencers = new()
		{
			{ BaseBossExt.DoggyOpponent, typeof(DoggyBossExt) },
			{ BaseBossExt.GrimoraOpponent, typeof(GrimoraBossExt) },
			{ BaseBossExt.KayceeOpponent, typeof(KayceeBossExt) },
			{ BaseBossExt.RoyalOpponent, typeof(RoyalBossExt) }
		};

		[HarmonyPrefix, HarmonyPatch(nameof(Opponent.SpawnOpponent))]
		public static bool Prefix(EncounterData encounterData, ref Opponent __result)
		{
			if (SaveManager.SaveFile.IsGrimora)
			{
				GameObject gameObject = new GameObject
				{
					name = "Opponent"
				};
				Opponent opponent = null;

				if (BossBattleSequencers.TryGetValue(encounterData.opponentType, out Type typeOut))
				{
					opponent = gameObject.AddComponent(typeOut) as Opponent;
				}
				else
				{
					opponent = gameObject.AddComponent<FinaleGrimoraOpponent>();
				}

				GrimoraMod.GrimoraPlugin.Log.LogDebug($"[Opponent.SpawnOpponent] Spawning opponent [{opponent}]");

				Opponent.Type opponentType = ProgressionData.LearnedMechanic(MechanicsConcept.OpponentQueue)
					? encounterData.opponentType
					: Opponent.Type.NoPlayQueue;

				string text = encounterData.aiId;
				if (string.IsNullOrEmpty(text))
				{
					text = "AI";
				}

				opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
				opponent.NumLives = opponent.StartingLives;
				opponent.OpponentType = opponentType;
				opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
				opponent.Blueprint = encounterData.Blueprint;
				opponent.Difficulty = encounterData.Difficulty;
				opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
				__result = opponent;

				GrimoraPlugin.Log.LogDebug($"EncounterData is [{encounterData}] Opponent result [{__result}]");
				return false;
			}

			return true;
		}
	}


	[HarmonyPatch(typeof(Part1BossOpponent))]
	public class Part1BossOpponentPatches
	{
		//StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);			// Begin game
		//StoryEventsData.SetEventCompleted(StoryEvent.FactoryConveyorBeltMoved); // Kaycee defeated
		//StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);			// Doggy defeated
		//StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);					// Grimora defeated

		[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.ReducePlayerLivesSequence))]
		public static void SetPlayerLivesToOne(Part1BossOpponent __instance)
		{
			RunState.Run.playerLives = 1;
		}

		[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.BossDefeatedSequence))]
		public static void Prefix(ref Part1BossOpponent __instance, out Part1BossOpponent __state)
		{
			__state = __instance;
		}

		[HarmonyPostfix, HarmonyPatch(nameof(Part1BossOpponent.BossDefeatedSequence))]
		public static IEnumerator Postfix(IEnumerator enumerator, Part1BossOpponent __state)
		{
			if (SaveManager.saveFile.IsGrimora)
			{
				if (__state is BaseBossExt bossExt)
				{
					bossExt.SetDefeated();
					GrimoraMod.GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] [PostFix] Boss defeated");
				}

				GrimoraMod.GrimoraPlugin.Log.LogDebug($"--> SaveFile is Grimora");
				GlitchOutAssetEffect.GlitchModel(
					GameObject.Find("Grimora_RightWrist").GetComponentsInChildren<Transform>().ToList()
						.Find(transform1 => transform1.gameObject.name.Contains("Mask")).gameObject.transform,
					true
				);
				AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX);
				GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");
				__state.DestroyScenery();
				__state.SetSceneEffectsShown(false);
				AudioController.Instance.StopAllLoops();
				yield return new WaitForSeconds(0.75f);
				__state.CleanUpBossBehaviours();
				//CustomCoroutine.WaitThenExecute(1f, new Action(LeshyAnimationController.Instance.HideArms), false);
				ViewManager.Instance.SwitchToView(View.Default, false, true);
				yield return new WaitForSeconds(0.8f);
				yield return new WaitForSeconds(0.25f);
				TurnManager.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
				yield break;
			}
			else
			{
				yield return enumerator;
			}
		}
	}
}