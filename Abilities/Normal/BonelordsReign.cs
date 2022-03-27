using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class BonelordsReign : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		GrimoraPlugin.Log.LogDebug($"[BonelordsReign] Player cards [{activePlayerCards.Count}]");
		if (activePlayerCards.Any())
		{
			yield return PreSuccessfulTriggerSequence();
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DID YOU REALLY THINK THE BONE LORD WOULD LET YOU OFF THAT EASILY?!"
			);
			foreach (var playableCard in activePlayerCards)
			{
				playableCard.Anim.StrongNegationEffect();
				int attack = playableCard.Attack == 0 ? 0 : 1 - playableCard.Attack;
				CardModificationInfo mod = new CardModificationInfo(attack, 1 - playableCard.Health);
				playableCard.AddTemporaryMod(mod);
				playableCard.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_BonelordsReign()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies attack and health is set to 1.";

		ApiUtils.CreateAbility<BonelordsReign>(rulebookDescription, "Bonelord's Reign");
	}
}
