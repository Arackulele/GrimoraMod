using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class Erratic : Strafe
{
	public const string RulebookName = "Erratic";

	public static Ability ability;
	public override Ability Ability => ability;

	private readonly RandomEx _rng = new();

	public override IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
	{
		bool toLeftIsValid = toLeft && toLeft.Card.SafeIsUnityNull();
		bool toRightIsValid = toRight && toRight.Card.SafeIsUnityNull();
		if (!toLeftIsValid)
		{
			movingLeft = false;
		}
		else if (!toRightIsValid)
		{
			movingLeft = true;
		}
		else
		{
			// means that both adj-slots are valid for moving to
			movingLeft = _rng.NextBoolean();
		}

		CardSlot destination = movingLeft ? toLeft : toRight;
		yield return StartCoroutine(
			MoveToSlot(
				destination,
				movingLeft
					? toLeftIsValid
					: toRightIsValid
			)
		);
		if (destination)
		{
			Log.LogDebug($"[Erratic] Attempting to move from slot [{Card.Slot.Index}] to slot [{destination.Index}]");
			yield return PreSuccessfulTriggerSequence();
			yield return LearnAbility();
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Erratic()
	{
		const string rulebookDescriptionEnglish = "At the end of the owner's turn, [creature] will move in a random direction.";
		const string rulebookDescriptionChinese = "持牌人回合结束时，[creature]将向随机方向移动。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Erratic>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Erratic.RulebookName)
		 .Build();
	}
}
