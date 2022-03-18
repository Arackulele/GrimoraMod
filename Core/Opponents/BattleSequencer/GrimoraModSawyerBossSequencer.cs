using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraModSawyerBossSequencer : GrimoraModBossBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GrimoraPlugin.GUID,
		nameof(GrimoraModSawyerBossSequencer),
		typeof(GrimoraModSawyerBossSequencer)
	);

	public override Opponent.Type BossType => BaseBossExt.SawyerOpponent;

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd;
	}


	public int bonesTakenCounter = 0;

	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		bonesTakenCounter++;

		if (bonesTakenCounter >= 2 && ResourcesManager.Instance.PlayerBones >= 3)
		{
			ViewManager.Instance.SwitchToView(View.BoneTokens, lockAfter: true);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"PLEASE, WON'T YOU SPARE SOME BONES FOR A POOR GHOUL LIKE ME?"
			);
			yield return new WaitForSeconds(0.75f);
			yield return ResourcesManager.Instance.SpendBones(1);
			yield return new WaitForSeconds(0.75f);
			bonesTakenCounter = 0;
			ViewManager.Instance.SetViewUnlocked();
		}
	}
}
