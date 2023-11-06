using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class GainAttackCandy : VariableStatBehaviour
{
	public const string RulebookName = "Wrath Of Halloween";
	
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public static StatIconManager.FullStatIcon FullStatIcon;

	public override SpecialStatIcon IconType => FullStatIcon.Id;

	public override int[] GetStatValues()
	{
		if (GrimoraModSawyerBossSequencer.CandyCounter > 0) return new[] { GrimoraModSawyerBossSequencer.CandyCounter, 0 };
		else return new[] { 0, 0 };
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_GainAttackCandy()
	{
		StatIconBuilder<GainAttackCandy>.Builder
		 .SetAppliesToHealth(false)
		 .SetIconGraphic(AssetUtils.GetPrefab<Texture>("ability_GainAttackCandy"))
		 .SetRulebookName(GainAttackCandy.RulebookName)
		 .SetRulebookDescription("A card bearing this sigil has 10 attack, 1 for each Candy Bucket Sawyer has left.")
		 .Build();
	}
}
