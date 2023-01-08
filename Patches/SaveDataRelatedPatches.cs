using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class SaveDataRelatedPatches
{
	public static bool IsNotGrimoraModRun => !IsGrimoraModRun;

	public static bool IsGrimoraModRun { get; set; }
}
