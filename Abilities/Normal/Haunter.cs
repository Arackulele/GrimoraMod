using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public class Haunter : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
	{
		return Card.Info.Abilities.Any(ab => ab != ability);
	}

	public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
	{
		if (Card.Info.abilities.Any(ab => ab != ability))
		{
			HauntedSlot hauntedSlot = HauntedSlot.SetupSlot(Card);
		}

		yield break;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Haunter()
	{
		const string rulebookDescription = "When [creature] perishes, it haunts the space it died in. "
		                                 + "The first creature played on this space gain its old sigils.";

		Texture icon = AbilitiesUtil.LoadAbilityIcon(Ability.Haunter.ToString());
		ApiUtils.CreateAbility<Haunter>(rulebookDescription, rulebookIcon: icon);
	}
}

public class HauntedSlot : NonCardTriggerReceiver
{
	[SerializeField] public CardSlot cardSlot;
	[SerializeField] public List<Ability> abilities = new();
	private CardModificationInfo _modInfo;

	public static HauntedSlot SetupSlot(PlayableCard playableCard)
	{
		Material defaultIconMat = playableCard.AbilityIcons.defaultIconMat;
		HauntedSlot hauntedSlot = new GameObject(playableCard.Slot.name + "_Haunted").AddComponent<HauntedSlot>();
		hauntedSlot.transform.SetParent(playableCard.Slot.transform);
		hauntedSlot.cardSlot = playableCard.Slot;
		hauntedSlot.abilities = playableCard.AbilityIcons.abilityIcons
		 .Where(abilityIconInteractable => abilityIconInteractable.Ability != Haunter.ability)
		 .ForEach(icon =>
			{
				AbilityIconInteractable hauntedIcon = Instantiate(
					icon,
					icon.transform.position + new Vector3(0, 0.1f),
					Quaternion.Euler(90, 0, 0),
					hauntedSlot.transform
				);
				hauntedIcon.gameObject.layer = icon.gameObject.layer;
				hauntedIcon.name = AbilityManager.AllAbilities.Find(fa => fa.Id == icon.Ability).Info.rulebookName;
				Renderer renderer = hauntedIcon.GetComponent<MeshRenderer>();
				renderer.enabled = true;
				// renderer.material.ChangeRenderMode(UnityObjectExtensions.BlendMode.Cutout);
				// this is so that it doesn't appear behind the card slot texture at the lowest point in the wave movement 
				// renderer.sortingOrder = 1;
				hauntedIcon.SetIcon(icon.GetComponent<Renderer>().material.mainTexture);

				SineWaveMovement wave = hauntedIcon.gameObject.AddComponent<SineWaveMovement>();
				wave.speed = 1;
				wave.xMagnitude = 0;
				wave.yMagnitude = 0.1f;
				wave.zMagnitude = 0;

				hauntedSlot.abilities.Add(icon.Ability);
			})
		 .Select(icon => icon.Ability)
		 .ToList();

		hauntedSlot._modInfo = new CardModificationInfo
		{
			abilities = new List<Ability>(hauntedSlot.abilities),
			fromCardMerge = false,
			fromTotem = false
		};

		return hauntedSlot;
	}

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		return otherCard.Slot == cardSlot;
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		otherCard.AddTemporaryMod(_modInfo);
		otherCard.Anim.PlayTransformAnimation();
		otherCard.UpdateStatsText();

		GrimoraPlugin.Log.LogDebug($"Destroying HauntedSlot [{this}]");
		CustomCoroutine.WaitThenExecute(0.1f, delegate() { Destroy(gameObject); });
		yield break;
	}
}
