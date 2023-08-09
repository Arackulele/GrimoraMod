using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public class LatchBoneDigger : Latch
{
	public const string RulebookName = "Latch Bone Digger";

	public static Ability ability;
	public override Ability Ability => ability;
	public override Ability LatchAbility => Ability.BoneDigger;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LatchBoneDigger()
	{
		const string rulebookDescriptionEnglish =
			"When [creature] perishes, its owner chooses a creature to gain the Bonedigger sigil.";
		const string rulebookDescriptionChinese =
			"[creature]阵亡时，其持牌人需选定下一个继承掘骨人印记的造物。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<LatchBoneDigger>.Builder
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("diggerlatcher2"))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(LatchBoneDigger.RulebookName)
		 .Build();
	}
}
