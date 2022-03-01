using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GainAttackBones : VariableStatBehaviour
{
	private static SpecialStatIcon SpecialStatIcon;

	public override SpecialStatIcon IconType => SpecialStatIcon;

	public static SpecialTriggeredAbility SpecialTriggeredAbility;

	public static NewSpecialAbility Create()
	{
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToAttack = true;
		info.appliesToHealth = false;
		info.rulebookName = "HellHound's Thirst";
		info.rulebookDescription = "[creature] gains 1 attack for each bone the player currently has.";
		info.iconGraphic = AllAbilityTextures.Single(_ => _.name.Equals("ability_GainAttackBones"));

		var sId = SpecialAbilityIdentifier.GetID(GUID, "GrimoraMod_GainAttackBones");

		var specialAbility = new NewSpecialAbility(typeof(GainAttackBones), sId, info);
		SpecialStatIcon = specialAbility.statIconInfo.iconType;
		SpecialTriggeredAbility = specialAbility.specialTriggeredAbility;

		return specialAbility;
	}

	public override int[] GetStatValues()
	{
		return new[] { ResourcesManager.Instance.PlayerBones, 0 };
	}
}
