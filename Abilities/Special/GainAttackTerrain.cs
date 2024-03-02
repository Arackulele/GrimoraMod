using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;


namespace GrimoraMod;

public class GainAttackTerrain : VariableStatBehaviour
{
	public const string RulebookName = "Inanimate Attack";

	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public static StatIconManager.FullStatIcon FullStatIcon;

	public override SpecialStatIcon IconType => FullStatIcon.Info.iconType;

	public override int[] GetStatValues()
	{
		int power = 0;

		foreach (var i in BoardManager.Instance.AllSlotsCopy)
		{
			if (i.Card != null && i.Card.Attack == 0 && i.Card != base.Card) power++;

		}

		return new[] { power, 0 };
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_GainAttackTerrain()
	{
		StatIconBuilder<GainAttackTerrain>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(AssetUtils.GetPrefab<Texture>("ability_GainAttackTerrain"))
		 .SetRulebookName(GainAttackTerrain.RulebookName)
		 .SetRulebookDescription("A card bearing this sigil gains 1 attack for every card that has 0 attack on the board.")
		 .Build();
	}
}
