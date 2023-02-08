using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod;

public class CreateArmyOfSkeletons : AbilityBehaviour
{
	public const string RulebookName = "Skeleton Horde";

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

		var openSlots = BoardManager.Instance.GetSlots(Card.Slot.IsPlayerSlot).OpenSlots();

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
		yield return slot.CreateCardInSlot("Skeleton".GetCardInfo(), 0.15f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_CreateArmyOfSkeletons()
	{
		const string rulebookDescriptionEnglish =
			"When [creature] is played, a Skeleton is created in each empty space on the owner's side. [define:Skeleton]";
		const string rulebookDescriptionChinese =
			"使用[creature]时，持牌人侧牌桌上所有空位均会出现骷髅卡牌。 [define:Skeleton]";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<CreateArmyOfSkeletons>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(CreateArmyOfSkeletons.RulebookName)
		 .Build();
	}
}
