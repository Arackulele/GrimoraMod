using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class CreateArmyOfSkeletons : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private const string CannotSpawnDialogue = "BLOCKED IN ALL SLOTS. NO ARMY THIS TIME.";

	private bool TranslatedDialogueIsNotEnglish()
	{
		return Localization.Translate(CannotSpawnDialogue) != CannotSpawnDialogue;
	}

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		ViewManager.Instance.SwitchToView(View.Board);

		var openSlots = BoardManager.Instance
			.GetSlots(Card.Slot.IsPlayerSlot)
			.Where(slot => slot.Card.IsNull())
			.ToList();

		foreach (var slot in openSlots)
		{
			yield return SpawnCardOnSlot(slot);
		}

		yield return PreSuccessfulTriggerSequence();
		if (openSlots.Count > 0)
		{
			LearnAbility();
		}
		else if (!HasLearned && (Localization.CurrentLanguage == Language.English || TranslatedDialogueIsNotEnglish()))
		{
			yield return TextDisplayer.Instance.ShowUntilInput(CannotSpawnDialogue, -0.65f, 0.4f);
		}
	}

	private IEnumerator SpawnCardOnSlot(CardSlot slot)
	{
		yield return BoardManager.Instance.CreateCardInSlot("Skeleton".GetCardInfo(), slot, 0.15f);
	}

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"When [creature] is played, a Skeleton is created in each empty space on the owner's side. [define:Skeleton]";

		return ApiUtils.CreateAbility<CreateArmyOfSkeletons>(rulebookDescription, "Skeleton Horde");
	}
}
