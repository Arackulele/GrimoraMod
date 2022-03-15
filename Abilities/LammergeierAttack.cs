using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class LammergeierAttack : VariableStatBehaviour
{
	public static NewSpecialAbility NewSpecialAbility;
	
	public static SpecialStatIcon SpecialStatIcon;

	public override SpecialStatIcon IconType => SpecialStatIcon;

	public static NewSpecialAbility Create()
	{
		StatIconInfo ogInfo = StatIconInfo.GetIconInfo(SpecialStatIcon.Bones);
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToHealth = false;
		info.iconGraphic = ogInfo.iconGraphic;
		info.rulebookName = ogInfo.name;
		info.rulebookDescription = ogInfo.rulebookDescription;

		var sId = SpecialAbilityIdentifier.GetID(GUID, "GrimoraMod_LammergeierAttack");

		NewSpecialAbility = new NewSpecialAbility(typeof(LammergeierAttack), sId, info);
		SpecialStatIcon = NewSpecialAbility.statIconInfo.iconType;
		return NewSpecialAbility;
	}

	public override int[] GetStatValues()
	{
		int attack = Mathf.FloorToInt(ResourcesManager.Instance.PlayerBones / 2f);
		return new[] { attack, 0 };
	}
}
