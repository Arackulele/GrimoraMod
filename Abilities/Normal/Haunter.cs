using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using InscryptionCommunityPatch;

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

		AbilityBuilder<Haunter>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.Haunter.ToString()))
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("haunter_pixel"))
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
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
				hauntedIcon.GetComponent<Renderer>().material.SetColor("_Color", GameColors.instance.yellow);

				//TODO: fix it for community patch
				if (hauntedIcon.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>() != null) UnityEngine.Object.Destroy(hauntedIcon.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>());
				if (hauntedIcon.GetComponent<AbilityIconInteractable>() != null) UnityEngine.Object.Destroy(hauntedIcon.GetComponent<AbilityIconInteractable>());
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
			abilities = new List<Ability>(hauntedSlot.abilities)
		};

		return hauntedSlot;
	}

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		return otherCard.Slot == cardSlot;
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		List<Ability> cardAllAbilities = otherCard.AllAbilities();


		if (cardAllAbilities.Count == 5)
		{
			yield return TextDisplayer.Instance.ShowUntilInput($"Oh... it will be rather difficult to haunt {otherCard.Info.DisplayedNameEnglish.LimeGreen()} with their abilities at max capacity.");
		}
		else
		{
			// All Abilities == 1, haunter slot abilities == 4
			// expected outcome: all 4 haunter abilities get added
		
			// All Abilities == 3, haunter slot abilities == 4, total == 7
			// expected outcome: first 2 haunter abilities get added, last one gets ditched
		
			// All Abilities == 4, haunter slot abilities == 2, total == 6
			// expected outcome: first haunter ability get added, last one gets ditched
			if (cardAllAbilities.Count + _modInfo.abilities.Count > 5)
			{
				List<Ability> abilitiesToAdd = _modInfo.abilities.GetRange(0, 5 - cardAllAbilities.Count);
				_modInfo.abilities = abilitiesToAdd;
			}
			if (otherCard != null) { 
			otherCard.AddTemporaryMod(_modInfo);
			otherCard.Anim.PlayTransformAnimation();
			otherCard.UpdateStatsText();
			otherCard.RenderCard();
			yield return new WaitForSeconds(0.1f);
			}
		}

		GrimoraPlugin.Log.LogDebug($"Destroying HauntedSlot [{this}]");
		CustomCoroutine.WaitThenExecute(0.1f, delegate { Destroy(gameObject); });
	}
}
