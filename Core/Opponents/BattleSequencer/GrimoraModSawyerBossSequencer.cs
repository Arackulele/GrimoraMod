using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraModSawyerBossSequencer : GrimoraModBossBattleSequencer
{
	public new static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GrimoraPlugin.GUID,
		nameof(GrimoraModSawyerBossSequencer),
		typeof(GrimoraModSawyerBossSequencer)
	);

	public override Opponent.Type BossType => SawyerBossOpponent.FullOpponent.Id;

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd;
	}

	public int bonesTakenCounter = 0;

	public static int sawyerbank = 2;

	private bool playedbankdialogue;

	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{

		bonesTakenCounter++;

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && TurnManager.Instance.Opponent.NumLives == 1)
		{

			if (bonesTakenCounter >= 2 && BoardManager.Instance.GetOpponentOpenSlots() != null)
			{

				if (sawyerbank > 0)
				{
					CardInfo selectedcard = SawyerBossOpponent.GetRandomCard(sawyerbank);
					yield return BoardManager.Instance.GetOpponentOpenSlots().GetRandomItem().CreateCardInSlot(selectedcard);
					sawyerbank -= selectedcard.bonesCost;
				}
				else
				{
					CardInfo selectedcard = SawyerBossOpponent.GetRandomCardSoul(ResourcesManager.Instance.PlayerEnergy);
					yield return BoardManager.Instance.GetOpponentOpenSlots().GetRandomItem().CreateCardInSlot(selectedcard);
					ViewManager.Instance.SwitchToView(View.Scales, lockAfter: true);
					yield return TextDisplayer.Instance.ShowUntilInput(
						"SPARE SOME SOULS?"
					);
					yield return new WaitForSeconds(0.75f);
					yield return ResourcesManager.Instance.SpendEnergy(selectedcard.energyCost);
					yield return new WaitForSeconds(0.75f);
					bonesTakenCounter = 0;
					ViewManager.Instance.SetViewUnlocked();
				}


			}
		}
		else
		{

			if (bonesTakenCounter >= 2 && ResourcesManager.Instance.PlayerBones >= 3)
			{
				ViewManager.Instance.SwitchToView(View.BoneTokens, lockAfter: true);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"PLEASE, WON'T YOU SPARE SOME BONES FOR A POOR GHOUL LIKE ME?"
				);
				yield return new WaitForSeconds(0.75f);
				yield return ResourcesManager.Instance.SpendBones(1);
				sawyerbank++;
				yield return new WaitForSeconds(0.75f);
				bonesTakenCounter = 0;
				ViewManager.Instance.SetViewUnlocked();
			}

		}
	}


	public override bool RespondsToOtherCardDie(
	PlayableCard card,
	CardSlot deathSlot,
	bool fromCombat,
	PlayableCard killer
)
	{
		return !card.IsPlayerCard();
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		sawyerbank++;
		if (playedbankdialogue == false && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls))
		{
			playedbankdialogue = true;
			yield return TextDisplayer.Instance.ShowUntilInput(
				"OH, I WILL STORE THIS BONE FOR LATER"
			);
		}
	}
}
