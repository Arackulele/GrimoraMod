using DiskCardGame;

namespace GrimoraMod;

public class GiantStrikeEnraged : GiantStrike
{
	public const string RulebookName = "Enraged Giant";

	public new static Ability ability;

	public override Ability Ability => ability;

	private void Awake()
	{
		Card.Anim.PlayTransformAnimation();

		Card.StatsLayer.SetEmissionColor(GameColors.Instance.red);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GiantStrikeEnraged()
	{
		const string rulebookDescriptionEnglish = "[creature] will strike each opposing space.";
		const string rulebookDescriptionChinese = "[creature]会攻击对面每个位置。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<GiantStrikeEnraged>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(GiantStrikeEnraged.RulebookName)
		 .Build();
	}
}
