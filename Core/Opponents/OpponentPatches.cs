using System;
using System.Collections;
using System.Collections.Generic;
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
			{ BaseBossExt.SawyerOpponent, typeof(SawyerBossOpponent) },
			{ BaseBossExt.GrimoraOpponent, typeof(GrimoraBossOpponentExt) },
			{ BaseBossExt.KayceeOpponent, typeof(KayceeBossOpponent) },
			{ BaseBossExt.RoyalOpponent, typeof(RoyalBossOpponentExt) }
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
			if (SaveManager.SaveFile.IsGrimora)
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
				GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] SaveFile is Grimora");

				if (__state is BaseBossExt bossExt)
				{
					SetBossDefeatedInConfig(__state, bossExt);

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] Glitching mask");
					GlitchOutAssetEffect.GlitchModel(
						bossExt.Mask.transform,
						true
					);

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] audio queue");
					AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX);

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] hiding skull");
					GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");

					if (bossExt is RoyalBossOpponentExt royalBossExt)
					{
						GrimoraAnimationController.Instance.SetHeadBool("face_disappointed", val: true);
						GrimoraAnimationController.Instance.SetHeadBool("face_happy", val: false);
						yield return new WaitForSeconds(0.5f);
						yield return royalBossExt.cannons.GetComponent<CannonTableEffects>().GlitchOutCannons();
						TableVisualEffectsManager.Instance.ResetTableColors();
					}

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] Destroying scenery");
					__state.DestroyScenery();

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] Set Scene Effects");
					__state.SetSceneEffectsShown(false);

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] Stopping audio");
					AudioController.Instance.StopAllLoops();

					yield return new WaitForSeconds(0.75f);

					GrimoraPlugin.Log.LogDebug($"[{__state.GetType()}] CleanUpBossBehaviours");
					__state.CleanUpBossBehaviours();

					ViewManager.Instance.SwitchToView(View.Default, false, true);

					GrimoraPlugin.Log.LogDebug($"Setting post battle special node to a rare code node data");
					TurnManager.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
				}

				yield break;
			}
			else
			{
				yield return enumerator;
			}
		}

		private static void SetBossDefeatedInConfig(Part1BossOpponent __state, BaseBossExt bossExt)
		{
			switch (bossExt)
			{
				case KayceeBossOpponent:
					GrimoraPlugin.ConfigKayceeFirstBossDead.Value = true;
					break;
				case SawyerBossOpponent:
					GrimoraPlugin.ConfigSawyerSecondBossDead.Value = true;
					break;
				case RoyalBossOpponentExt:
					GrimoraPlugin.ConfigRoyalThirdBossDead.Value = true;
					break;
				case GrimoraBossOpponentExt:
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