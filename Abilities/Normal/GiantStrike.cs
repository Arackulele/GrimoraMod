using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using UnityEngine;

namespace GrimoraMod;

public class GiantStrike : AbilityBehaviour, IGetOpposingSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => true;

	private void Awake()
	{
		if (Card.Anim is GravestoneCardAnimationController && Card.transform.Find("SkeletonArms_Giants").IsNull())
		{
			GrimoraPlugin.Log.LogDebug($"Adding skeleton arm giant prefab to card [{Card.InfoName()}]");
			Animator skeletonArm2Attacks = UnityObject.Instantiate(
					AssetUtils.GetPrefab<GameObject>("SkeletonArms_Giants"),
					Card.transform
				).GetComponent<Animator>();
			skeletonArm2Attacks.name = "SkeletonArms_Giants";
			skeletonArm2Attacks.gameObject.AddComponent<AnimMethods>();
			skeletonArm2Attacks.gameObject.SetActive(false);
		}
	}

	public List<CardSlot> GetTwinGiantOpposingSlots()
	{
		return BoardManager.Instance.PlayerSlotsCopy
		 .Where(slot => slot.opposingSlot.Card == Card)
		 .ToList();
	}

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		// assume giant is in slot indexes 0, 1
		// original slots has opposing slot of index 1
		List<CardSlot> slotsToTarget = new List<CardSlot>(GetTwinGiantOpposingSlots());
		if (slotsToTarget.Exists(slot => slot.Card))
		{
			List<CardSlot> slotsWithCards = slotsToTarget
			 .Where(slot => slot.Card)
			 .ToList();
			if (slotsWithCards.Count == 1)
			{
				slotsToTarget.Clear();
				slotsToTarget.Add(slotsWithCards[0]);
				// single card has health greater than current attack, then attack twice 
				if (slotsWithCards[0].Card.Health > Card.Attack)
				{
					slotsToTarget.Add(slotsWithCards[0]);
				}
			}
		}

		GrimoraPlugin.Log.LogInfo($"[{GetType().Name}] Opposing slots is now [{slotsToTarget.Join(slot => slot.Index.ToString())}]");
		return slotsToTarget;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GiantStrike()
	{
		const string rulebookDescription =
			"[creature] will strike each opposing space. "
		+ "If only one creature is in the opposing spaces, this card will strike that creature twice. ";

		AbilityBuilder<GiantStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
