using DiskCardGame;
using InscryptionCommunityPatch;
using HarmonyLib;
using Pixelplacement;
using System.Collections;
using UnityEngine;
using GrimoraMod.Saving;

namespace GrimoraMod;

[HarmonyPatch(typeof(InscryptionCommunityPatch.ResourceManagers.EnergyDrone))]
public class EnergyDrone
{

	[HarmonyPrefix, HarmonyPatch(nameof(InscryptionCommunityPatch.ResourceManagers.EnergyDrone.SceneCanHaveEnergyDrone))]
	public static bool SceneCanHaveEnergyDrone(string sceneName)
	{
		if (GrimoraSaveUtil.IsGrimoraModRun) return false;
		else return true;
	}


}
