using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;
using GrimoraMod.Saving;

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
}
