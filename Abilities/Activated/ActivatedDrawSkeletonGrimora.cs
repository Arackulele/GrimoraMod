using DiskCardGame;

namespace GrimoraMod;

public class ActivatedDrawSkeletonGrimora : ActivatedDrawSkeleton
{
	public const string RulebookName = "Disinter";
	
	public static Ability ability;

	public override Ability Ability => ability;

	public override int BonesCost => 2;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ActivatedDrawSkeletonGrimora()
	{
		const string rulebookDescriptionEnglish = "Pay 2 Bones to create a Skeleton in your hand.";
		const string rulebookDescriptionChinese = "消耗2根骨头，在你的手牌中创造一张骷髅卡牌。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<ActivatedDrawSkeletonGrimora>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedDrawSkeletonGrimora.RulebookName)
		 .Build();
	}
}
