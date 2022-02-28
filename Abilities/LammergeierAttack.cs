using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class LammergeierAttack : VariableStatBehaviour
{
	public static readonly NewSpecialAbility NewSpecialAbility = Create();

	public override SpecialStatIcon IconType => SpecialStatIcon.Bones;

	public static NewSpecialAbility Create()
	{
		StatIconInfo info = (StatIconInfo)Internal_CloneSingle(StatIconInfo.GetIconInfo(SpecialStatIcon.Bones));
		Log.LogDebug($"StatIconInfo is null {info}");
		info.appliesToHealth = false;
		info.rulebookName = "Lammergeier(Attack)";

		var sId = SpecialAbilityIdentifier.GetID(GUID, "GrimoraMod_LammergeierAttack");

		NewSpecialAbility ability = new NewSpecialAbility(typeof(LammergeierAttack), sId, info);
		return ability;
	}

	public override int[] GetStatValues()
	{
		int attack = Mathf.FloorToInt(ResourcesManager.Instance.PlayerBones / 2f);
		return new[] { attack, 0 };
	}
}
