using DiskCardGame;
using InscryptionAPI.Card;

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
		StatIconBuilder<GainAttackBones>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(AllAbilityTextures.Single(_ => _.name.Equals("ability_GainAttackBones")))
		 .SetRulebookName(GainAttackBones.RulebookName)
		 .SetRulebookDescription("[creature] gains 1 attack for each bone the player currently has.")
		 .Build();
	}
}
