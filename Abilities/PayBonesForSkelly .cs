using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class PayBonesForSkeleton : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
	public override int BonesCost => 2;
	public override IEnumerator Activate()
	{
		yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Skeleton"), 0.25f);
		yield break;
	}

	public static NewAbility CreatePayBonesForSkeleton()
		{
		const string rulebookDescription =
			"Pay 2 Bones for [creature] to summon a Skeleton in your Hand " +
			"Rise, Army, RIIISE.";

		return ApiUtils.CreateAbility<PayBonesForSkeleton>(
			GrimoraPlugin.AllSpriteAssets.Single(spr => spr.name == "PayBonesForSkeleton").texture,
			nameof(PayBonesForSkeleton),
			rulebookDescription,
		5,
		activated: true
		);
	}
}
