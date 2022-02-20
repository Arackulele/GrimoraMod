using System.Collections;
using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace GrimoraMod;

public class GainAttackBones : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;


	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
	}


	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		//clearing mods so it doesnt stack
		Card.TemporaryMods.Clear();

		new WaitForSeconds(0.1f);

		int boneamount = Singleton<ResourcesManager>.Instance.PlayerBones;
		Card.AddTemporaryMod(new CardModificationInfo(boneamount, 0));

		GrimoraPlugin.Log.LogWarning("Bone Damage Triggered");
		yield break;
	}

	public static NewAbility Create()
		{
			const string rulebookDescription =
				"[creature] gains 1 attack for each bone the player currently has.";

			return ApiUtils.CreateAbility<GainAttackBones>(
				rulebookDescription, "GainAttackBones"
			);
	} 

}
