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

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		bool isBonehoundOnBoard = BoardManager.Instance.opponentSlots.Exists(info => info.name.Equals("Bonehound"));
		if (new RandomEx().NextBoolean() && isBonehoundOnBoard && ResourcesManager.Instance.PlayerBones > 5)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"PLEASE, WON'T YOU SPARE SOME BONES FOR [c:R]BONEHOUND[c:]?",
				-0.65f,
				0.4f
			);
			ViewManager.Instance.SwitchToView(View.BoneTokens);
			yield return new WaitForSeconds(0.1f);
			yield return ResourcesManager.Instance.SpendBones(1);
			yield return new WaitForSeconds(1f);
		}
	}
}
