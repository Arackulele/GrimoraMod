using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class PayBonesForSkeleton : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
	public override int BonesCost => 1;

	public override IEnumerator Activate()
	{
		yield return CardSpawner.Instance.SpawnCardToHand("Skeleton".GetCardInfo());
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"Pay 2 Bones for [creature] to summon a Skeleton in your hand. " +
			"Rise, Army, RIIISE.";

		return ApiUtils.CreateAbility<PayBonesForSkeleton>(rulebookDescription, activated: true);
	}
}
