using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEctoplasm = $"{GUID}_Ectoplasm";

	private void Add_Card_Extoplasm()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("ectoplasm"), AssetUtils.GetPrefab<Sprite>("ectoplasm_emission"))
			.SetAbilities(ActivatedEnergyDrawWyvern.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("THE ESSENCE OF A SPIRIT, HIDDEN IN EVERY CORNER AND EVERY SHADOW. ONCE YOU SEE ONE, YOULL FIND THE OTHERS SOON ENOUGH.")
			.SetNames(NameEctoplasm, "Ectoplasm")
			.Build();
	}
}
