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
		StatIconInfo ogInfo = StatIconInfo.GetIconInfo(SpecialStatIcon.Bones);

		StatIconBuilder<LammergeierAttack>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(ogInfo.iconGraphic)
		 .SetRulebookName(LammergeierAttack.RulebookName)
		 .SetRulebookDescription(ogInfo.rulebookDescription)
		 .Build();
	}
}
