using DiskCardGame;

namespace GrimoraMod;

public class Boneless : AbilityBehaviour
{
	public const string RulebookName = "Boneless";

	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Boneless()
	{
		const string rulebookDescriptionEnglish = "[creature] yields no bones upon death.";
		const string rulebookDescriptionChinese = "[creature]死亡时，不会获得骨头。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Boneless>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Boneless.RulebookName)
		 .Build();
	}
}
