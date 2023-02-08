using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class LammergeierAttack : VariableStatBehaviour
{
	public const string RulebookName = "One Half Bones";
	
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public static StatIconManager.FullStatIcon FullStatIcon;
	public override SpecialStatIcon IconType => FullStatIcon.Info.iconType;

	public override int[] GetStatValues()
	{
		int attack = Mathf.FloorToInt(ResourcesManager.Instance.PlayerBones / 2f);
		return new[] { attack, 0 };
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_LammergeierAttack()
	{
		const string rulebookDescriptionEnglish = "The power of [creature] is equal to half of the Bones of the owner.";
		const string rulebookDescriptionChinese = "[creature]的力量等于持牌人持有兽骨数量的一半。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		StatIconInfo ogInfo = StatIconInfo.GetIconInfo(SpecialStatIcon.Bones);

		StatIconBuilder<LammergeierAttack>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(ogInfo.iconGraphic)
		 .SetRulebookName(LammergeierAttack.RulebookName)
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
