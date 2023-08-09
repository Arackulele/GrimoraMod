using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public class ActivatedEnergyDrawWyvern : ActivatedAbilityBehaviour
{
	private const int ENERGY_COST = 2;

	public const string RulebookName = "Materialize";

	public static Ability ability;
	public override Ability Ability => ability;

	public override int EnergyCost => ENERGY_COST;

	public override IEnumerator Activate()
	{
		CardInfo cloneInfo = Card.Info.Clone() as CardInfo;
		cloneInfo.Mods = new(Card.Info.Mods);
		yield return CardSpawner.Instance.SpawnCardToHand(cloneInfo);
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_ActivatedEnergyDrawWyvern()
	{
		const string rulebookDescriptionEnglish = "Pay 2 Souls for [creature] to summon a copy in your hand.";
		const string rulebookDescriptionChinese = "消耗2点魂能，[creature]会向你的手牌中添加一张自己的复制品。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<ActivatedEnergyDrawWyvern>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedEnergyDrawWyvern.RulebookName)
		 .Build();
	}
}
