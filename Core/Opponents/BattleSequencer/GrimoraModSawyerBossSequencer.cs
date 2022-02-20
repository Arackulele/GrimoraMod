using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraModSawyerBossSequencer : GrimoraModBossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.SawyerOpponent;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd;
	}


	public int bonestakenCounter = 0;
	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		bonestakenCounter++;

		if ( bonestakenCounter == 2)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"PLEASE, WON'T YOU SPARE SOME BONES FOR A POOR GHOUL LIKE ME?",
				-0.65f,
				0.4f
			);
			ViewManager.Instance.SwitchToView(View.BoneTokens);
			yield return new WaitForSeconds(0.4f);
			yield return ResourcesManager.Instance.SpendBones(1);
			bonestakenCounter = 0;
		}
	}
}
