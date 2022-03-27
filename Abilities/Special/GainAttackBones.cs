using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GainAttackBones : VariableStatBehaviour
{
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
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToHealth = false;
		info.rulebookName = "HellHound's Thirst";
		info.rulebookDescription = "[creature] gains 1 attack for each bone the player currently has.";
		info.iconGraphic = AllAbilityTextures.Single(_ => _.name.Equals("ability_GainAttackBones"));

		ApiUtils.CreateSpecialAbility<GainAttackBones>(info.rulebookName);
		ApiUtils.CreateStatIcon<GainAttackBones>(info);
	}
}
