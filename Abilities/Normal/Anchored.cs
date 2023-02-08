using DiskCardGame;

namespace GrimoraMod;

public class Anchored : AbilityBehaviour
{
	public const string RulebookName = "Anchored";

	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Anchored()
	{
		const string rulebookDescriptionEnglish = "[creature] is unaffected by the motion of the ship.";
		const string rulebookDescriptionChinese = "[creature]不会受到船动作的影响。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Anchored>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Anchored.RulebookName)
		 .Build();
	}
}
