using DiskCardGame;
using UnityEngine;
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
		const string rulebookDescription = "Pay 2 Bones to create a Skeleton in your hand.";
		
		AbilityBuilder<ActivatedDrawSkeletonGrimora>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("pixelability_activated_drawskeletons"))
		 .SetRulebookName(ActivatedDrawSkeletonGrimora.RulebookName)
		 .Build();
	}
}
