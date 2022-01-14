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

				GrimoraPlugin.Log.LogDebug($"[Opponent.SpawnOpponent] Spawning opponent [{opponent}]");

				string text = encounterData.aiId;
				if (string.IsNullOrEmpty(text))
				{
					text = "AI";
				}

				opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
				opponent.NumLives = opponent.StartingLives;
				opponent.OpponentType = encounterData.opponentType;
				opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
				opponent.Blueprint = encounterData.Blueprint;
				opponent.Difficulty = encounterData.Difficulty;
				opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
				__result = opponent;

				GrimoraPlugin.Log.LogDebug($"[Opponent.SpawnOpponent] Opponent result [{__result}]");
				return false;
			}

			return true;
		}
	}


	[HarmonyPatch(typeof(Part1BossOpponent))]
	public class Part1BossOpponentPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.ReducePlayerLivesSequence))]
		public static void SetPlayerLivesToOne(Part1BossOpponent __instance)
		{
			if(SaveManager.SaveFile.IsGrimora)
			{
				RunState.Run.playerLives = 1;
			}
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
				GrimoraPlugin.Log.LogDebug($"> SaveFile is Grimora");

				if (__state is BaseBossExt bossExt)
				{
					HandleBossExtensions(__state, bossExt);
				}

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

				ViewManager.Instance.SwitchToView(View.Default, false, true);

				RunState.Run.playerLives = 2;

				GrimoraPlugin.Log.LogDebug($"Setting post battle special node to a rare code node data");
				TurnManager.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
				yield break;
			}
			else
			{
				yield return enumerator;
			}
		}

		private static void HandleBossExtensions(Part1BossOpponent __state, BaseBossExt bossExt)
		{
			switch (bossExt)
			{
				case KayceeBossExt:
					GrimoraPlugin.ConfigKayceeFirstBossDead.Value = true;
					break;
				case DoggyBossExt:
					GrimoraPlugin.ConfigDoggySecondBossDead.Value = true;
					break;
				case RoyalBossExt:
					GrimoraPlugin.ConfigRoyalThirdBossDead.Value = true;
					break;
				case GrimoraBossExt:
					GrimoraPlugin.ConfigGrimoraBossDead.Value = true;
					break;
			}

			var bossPiece = ChessboardMapExt.Instance.BossPiece;
			ChessboardMapExt.Instance.BossDefeated = true;
			ChessboardMapExt.Instance.AddPieceToRemovedPiecesConfig(bossPiece.name);
			GrimoraPlugin.Log.LogDebug($"[Part1BossOpponent.BossDefeatedSequence][PostFix]" +
			                           $" Boss {__state.GetType()} defeated.");
		}
	}
}