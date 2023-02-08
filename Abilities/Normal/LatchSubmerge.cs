using DiskCardGame;

namespace GrimoraMod;

public class LatchSubmerge : Latch
{
	public const string RulebookName = "Latch Submerge";

	public static Ability ability;
	public override Ability Ability => ability;
	public override Ability LatchAbility => Ability.Submerge;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LatchSubmerge()
	{
		const string rulebookDescriptionEnglish =
			"When [creature] perishes, its owner chooses a creature to gain the Waterborne sigil.";
		const string rulebookDescriptionChinese =
			"[creature]阵亡时，其持牌人需选定下一个继承水袭印记的造物。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<LatchSubmerge>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(LatchSubmerge.RulebookName)
		 .Build();
	}
}
