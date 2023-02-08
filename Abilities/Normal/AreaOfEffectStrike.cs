using DiskCardGame;

namespace GrimoraMod;

public class AreaOfEffectStrike : StrikeAdjacentSlots
{
	public const string RulebookName = "Area Of Effect Strike";

	public static Ability ability;
	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;

}

public partial class GrimoraPlugin
{
	public void Add_Ability_AreaOfEffectStrike()
	{
		const string rulebookDescriptionEnglish =
			"[creature] will strike its adjacent slots, and each opposing space to the left, right, and center of it.";
		const string rulebookDescriptionChinese = 
			"[creature]会攻击相邻位置，以及正对面的左右两侧和中间位置各一次。 ";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<AreaOfEffectStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(AreaOfEffectStrike.RulebookName)
		 .Build();
	}
}
