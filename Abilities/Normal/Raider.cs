using DiskCardGame;

namespace GrimoraMod;

public class Raider : StrikeAdjacentSlots
{
	public const string RulebookName = "Raider";

	public static Ability ability;

	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Raider()
	{
		const string rulebookDescriptionEnglish = "[creature] will strike its adjacent slots, except other Raiders.";
		const string rulebookDescriptionChinese = "[creature]会攻击相邻位置中没有其他的[creature]的位置。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Raider>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Raider.RulebookName)
		 .Build();
	}
}
