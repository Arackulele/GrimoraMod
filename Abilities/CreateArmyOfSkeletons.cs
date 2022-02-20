using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class CreateArmyOfSkeletons : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private const string CannotSpawnDialogue = "BLOCKED IN ALL SLOTS. NO ARMY THIS TIME.";

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		ViewManager.Instance.SwitchToView(View.Board);
		var slots = base.Card.Slot.IsPlayerSlot
			? BoardManager.Instance.PlayerSlotsCopy
			: BoardManager.Instance.OpponentSlotsCopy;
		
		foreach (var slot in slots.Where(slot => slot.Card is null))
		{
			yield return SpawnCardOnSlot(slot);
		}
		
		yield return PreSuccessfulTriggerSequence();
		if (slots.Count > 0)
		{
			LearnAbility();
		} 
		else if (!HasLearned && (Localization.CurrentLanguage == Language.English || Localization.Translate(CannotSpawnDialogue) != CannotSpawnDialogue))
		{
			yield return TextDisplayer.Instance.ShowUntilInput(CannotSpawnDialogue, -0.65f, 0.4f);
		}
	}
	
	private IEnumerator SpawnCardOnSlot(CardSlot slot)
	{
		yield return BoardManager.Instance.CreateCardInSlot("Skeleton".GetCardInfo(), slot, 0.15f);
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"When [creature] is played, a Skeleton is created in each empty space on the owner's side. [define:Skeleton]";

		return ApiUtils.CreateAbility<CreateArmyOfSkeletons>(rulebookDescription);
	}
}
