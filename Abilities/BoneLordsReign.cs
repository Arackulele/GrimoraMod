using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class BoneLordsReign : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;


	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}
	public override IEnumerator OnResolveOnBoard()
	{
	yield return base.PreSuccessfulTriggerSequence();
		foreach (CardSlot cardSlot in Singleton<BoardManager>.Instance.opponentSlots)
			if (cardSlot.Card != null)
			{
				Card.AddTemporaryMod(new CardModificationInfo(1 - Card.Attack, 0));
				Card.Anim.StrongNegationEffect();
			}
	}

		public static NewAbility CreateBoneLordsReign()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies Power is set to 1. " +
			"When the Bone Lord appears, every Creature will fall.";

		return ApiUtils.CreateAbility<BoneLordsReign>(
			GrimoraPlugin.AllSpriteAssets.Single(spr => spr.name == "BoneLordsReign").texture,
			nameof(BoneLordsReign),
			rulebookDescription,
			5
		);
	}
}
