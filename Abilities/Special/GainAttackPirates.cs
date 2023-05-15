using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;


namespace GrimoraMod;

public class GainAttackPirates : VariableStatBehaviour
{
	public const string RulebookName = "Trusty ol' Crew";

	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public static StatIconManager.FullStatIcon FullStatIcon;

	public override SpecialStatIcon IconType => FullStatIcon.Info.iconType;

	public override int[] GetStatValues()
	{
		int power = 0;

		foreach (var i in BoardManager.Instance.AllSlotsCopy)
		{
			if (i.Card != null && i.Card.AllAbilities().Contains(Anchored.ability)) power++;

		}

		return new[] { power, 0 };
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_GainAttackPirates()
	{
		StatIconBuilder<GainAttackPirates>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(AssetUtils.GetPrefab<Texture>("ability_GainAttackPirates"))
		 .SetPixelIconGraphic(AssetUtils.GetPrefab<Sprite>("ability_GainAttackPirates_pixel"))
		 .SetRulebookName(GainAttackPirates.RulebookName)
		 .SetRulebookDescription("A card bearing this sigil gains 1 attack for every pirate on the board.")
		 .Build();
	}
}
