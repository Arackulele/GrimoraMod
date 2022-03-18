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

	public static StatIconManager.FullStatIcon Create()
	{
		StatIconInfo ogInfo = StatIconInfo.GetIconInfo(SpecialStatIcon.Bones);
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToHealth = false;
		info.iconGraphic = ogInfo.iconGraphic;
		info.rulebookName = "One Half Bones";
		info.rulebookDescription = ogInfo.rulebookDescription;

		FullSpecial = ApiUtils.CreateSpecialAbility<LammergeierAttack>();
		FullStatIcon = ApiUtils.CreateStatIcon<LammergeierAttack>(info);
		return FullStatIcon;
	}

	public override int[] GetStatValues()
	{
		int attack = Mathf.FloorToInt(ResourcesManager.Instance.PlayerBones / 2f);
		return new[] { attack, 0 };
	}
}
