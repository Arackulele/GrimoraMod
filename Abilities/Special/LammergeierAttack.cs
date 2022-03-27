using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class LammergeierAttack : VariableStatBehaviour
{
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
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToHealth = false;
		info.iconGraphic = ogInfo.iconGraphic;
		info.rulebookName = "One Half Bones";
		info.rulebookDescription = ogInfo.rulebookDescription;

		ApiUtils.CreateSpecialAbility<LammergeierAttack>();
		ApiUtils.CreateStatIcon<LammergeierAttack>(info);
	}
}
