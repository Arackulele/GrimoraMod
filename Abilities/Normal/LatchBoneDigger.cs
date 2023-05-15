using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public class LatchBoneDigger : Latch
{
	public static Ability ability;
	public override Ability Ability => ability;
	public override Ability LatchAbility => Ability.BoneDigger;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LatchBoneDigger()
	{
		const string rulebookDescription =
			"When [creature] perishes, its owner chooses a creature to gain the Bonedigger sigil.";

		AbilityBuilder<LatchBoneDigger>.Builder
			.SetPixelIcon(AssetUtils.GetPrefab<Sprite>("diggerlatcher2"))
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
