using System.Collections;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class CombatPhaseManagerPatches
{
	private static readonly int MAGIC_LIFE = 10;
	
	[HarmonyTargetMethods]
	static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
	{
		// StrikeCardSlot is the IEnumerator method, but there's a hidden compiler class, <StrikeCardSlot>d__2,
		//	that actually has all the byte code to look for.
		Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.CombatPhaseManager+<DoCombatPhase>d__4");
		Log.LogDebug($"Enumerator type [{getEnumeratorType}]");
		return Enumerable.Where(AccessTools.GetDeclaredMethods(getEnumeratorType), m =>
		{
			Log.LogDebug($"Method name is [{m.Name}]");
			return m.Name == "MoveNext";
		});
	}

	[HarmonyTranspiler]
	internal static System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> Transpiler(
		System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> instructions)
	{
		bool skipFirst = false;
		foreach (var codeInstruction in instructions)
		{
			var opcode = codeInstruction.opcode;
			if (opcode == System.Reflection.Emit.OpCodes.Ldc_I4_5)
			{
				if (!skipFirst)
				{
					skipFirst = true;
				}
				else
				{
					Log.LogDebug($"Opcode is [{opcode}] operand [{codeInstruction.operand}]");
					codeInstruction.opcode = System.Reflection.Emit.OpCodes.Ldc_I4;
					codeInstruction.operand = MAGIC_LIFE;
				}
			}

			yield return codeInstruction;
		}
	}
}
