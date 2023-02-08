using DiskCardGame;

namespace GrimoraMod;

public class LooseLimb : TailOnHit
{
	public const string RulebookName = "Loose Limb";

	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LooseLimb()
	{
		const string rulebookDescriptionEnglish =
			"When [creature] would be struck, a severed limb is created in its place and this card moves into an adjacent open slot.";
		const string rulebookDescriptionChinese =
			"当[creature]有可能受到攻击时，会在原地留下残肢，自身则会向相邻空位移动。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<LooseLimb>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(LooseLimb.RulebookName)
		 .Build();
	}
}
