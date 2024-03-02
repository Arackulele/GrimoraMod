using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;
using GrimoraMod.Saving;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod;

[HarmonyPatch(typeof(TurnManager))]
public class TurnManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.SetupPhase))]
	public static IEnumerator PostfixAddStartingBones(
		IEnumerator enumerator,
		TurnManager __instance,
		EncounterData encounterData
	)
	{
		yield return enumerator;
		if (ConfigHelper.Instance.IsDevModeEnabled)
		{
			yield return ResourcesManager.Instance.AddBones(25);
		}

		bool drawPile3DIsActive = CardDrawPiles3D.Instance && CardDrawPiles3D.Instance.pile;

		if (GrimoraRunState.CurrentRun.riggedDraws.Contains("Boon_StartingDraw"))
		{
			if (drawPile3DIsActive)
			{
				CardDrawPiles3D.Instance.pile.Draw();
				yield return CardDrawPiles.Instance.DrawCardFromDeck();
			}
		}

		if (GrimoraRunState.CurrentRun.riggedDraws.Contains("Boon_MaxEnergy"))
		{
			yield return ResourcesManager.Instance.AddMaxEnergy(1);
			yield return ResourcesManager.Instance.AddEnergy(1);
		}

		if (SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards) && TurnManager.Instance.Opponent is BaseBossExt)
		{
			yield return ResourcesManager.Instance.AddBones(1);
			yield return ResourcesManager.Instance.AddMaxEnergy(1);
			yield return ResourcesManager.Instance.AddEnergy(1);

		}

		if (GrimoraRunState.CurrentRun.riggedDraws.Contains("Boon_BossBones") && TurnManager.Instance.Opponent is BaseBossExt)
		{

			yield return ResourcesManager.Instance.AddBones(3);

		}

		if (GrimoraRunState.CurrentRun.riggedDraws.Contains("Boon_TerrainSpawn")) yield return BoardManager.Instance.GetPlayerOpenSlots().GetRandomItem().CreateCardInSlot(NameDeadTree.GetCardInfo());

		if (!SaveFile.IsAscension ||( SaveFile.IsAscension && !AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones)))
		{
			int bonesToAdd = GrimoraRunState.CurrentRun.regionTier;
			if (GrimoraRunState.CurrentRun.riggedDraws != null) bonesToAdd -= GrimoraRunState.CurrentRun.riggedDraws.Count();
			Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
			yield return ResourcesManager.Instance.AddBones(Math.Max(bonesToAdd, 0));
		}
		else
		{
			//only show activation when No Bones is changing something
			if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones) && GrimoraRunState.CurrentRun.regionTier > 0) ChallengeActivationUI.TryShowActivation(ChallengeManagement.NoBones);
			Log.LogInfo($"{SaveFile.IsAscension } +  {AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones)}");
		}
	}

	[HarmonyPatch(typeof(TurnManager), nameof(TurnManager.DoUpkeepPhase))]
	[HarmonyPostfix]
	private static IEnumerator TurnManager_UpkeepPhase(IEnumerator sequence, bool playerUpkeep)
	{

		yield return sequence;

		if (playerUpkeep && GrimoraSaveUtil.IsGrimoraModRun)
		{ 
		bool showEnergyModule = !ResourcesManager.Instance.EnergyAtMax || ResourcesManager.Instance.PlayerEnergy < ResourcesManager.Instance.PlayerMaxEnergy;
		if (showEnergyModule)
		{
			ViewManager.Instance.SwitchToView(View.Default, false, true);
			yield return new UnityEngine.WaitForSeconds(0.1f);
		}

		yield return ResourcesManager.Instance.AddMaxEnergy(1);
		yield return ResourcesManager.Instance.RefreshEnergy();

		if (showEnergyModule)
		{
			yield return new UnityEngine.WaitForSeconds(0.25f);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
		}
		}
	}


	}
