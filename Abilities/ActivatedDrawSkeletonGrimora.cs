using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class ActivatedDrawSkeletonGrimora : ActivatedDrawSkeleton
{
	public static readonly NewAbility NewAbility = Create();

	public static readonly Ability ability;

	public override Ability Ability => ability;
	
	public override int BonesCost => 2;

	public static NewAbility Create()
	{
		const string rulebookDescription = "Pay 2 Bones to create a Skeleton in your hand.";

		return ApiUtils.CreateAbility<ActivatedDrawSkeletonGrimora>
			(rulebookDescription, "Disinter", true);
	}
}
