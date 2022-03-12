using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class LammergeierAttack : VariableStatBehaviour
{
	public static SpecialTriggeredAbility SpecialTriggeredAbility;

	public override SpecialStatIcon IconType => SpecialStatIcon.Bones;

	public static NewSpecialAbility Create()
	{
		StatIconInfo info = (StatIconInfo)Internal_CloneSingle(StatIconInfo.GetIconInfo(SpecialStatIcon.Bones));
		info.appliesToHealth = false;
		info.rulebookName = "Lammergeier(Attack)";

		var sId = SpecialAbilityIdentifier.GetID(GUID, "GrimoraMod_LammergeierAttack");

		NewSpecialAbility ability = new NewSpecialAbility(typeof(LammergeierAttack), sId, info);
		SpecialTriggeredAbility = ability.specialTriggeredAbility;
		return ability;
	}

	public override int[] GetStatValues()
	{
		int attack = Mathf.FloorToInt(ResourcesManager.Instance.PlayerBones / 2f);
		return new[] { attack, 0 };
	}
}
