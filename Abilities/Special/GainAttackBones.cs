using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class GainAttackBones : VariableStatBehaviour
{
	public const string RulebookName = "HellHound's Thirst";
	
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public static StatIconManager.FullStatIcon FullStatIcon;

	public override SpecialStatIcon IconType => FullStatIcon.Id;

	public override int[] GetStatValues()
	{
		return new[] { ResourcesManager.Instance.PlayerBones, 0 };
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_GainAttackBones()
	{
		const string rulebookDescriptionEnglish = "[creature] gains 1 attack for each bone the player currently has.";
		const string rulebookDescriptionChinese = "玩家每有一根骨头，带有该印记的卡牌会增加1点力量。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		StatIconBuilder<GainAttackBones>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(AssetUtils.GetPrefab<Texture>("ability_GainAttackBones"))
		 .SetRulebookName(GainAttackBones.RulebookName)
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
