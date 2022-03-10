using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GrimoraMod;

public class Haunter : AbilityBehaviour
{
	public static Ability ability;
	private static readonly int Color1 = Shader.PropertyToID("_Color");

	public override Ability Ability => ability;

	public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
	{
		return Card.Info.Abilities.FindAll(ab => ab != Haunter.ability).Count > 0;
	}

	public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
	{
		List<GameObject> createdSigils = new List<GameObject>();
		foreach (AbilityIconInteractable icon in Card.GetComponentInChildren<CardAbilityIcons>().abilityIcons)
		{
			if (icon.Ability == Haunter.ability) continue;

			//Get and Spawn Sigils
			GameObject sigils = UnityEngine.Object.Instantiate(
				icon.transform.gameObject,
				icon.transform.position + new Vector3(0, 0.1f, 0),
				icon.transform.rotation
			);
			foreach (Renderer rend in sigils.GetComponentsInChildren<Renderer>())
			{
				rend.enabled = true;
				rend.material = new Material(rend.material);
			}

			Renderer sigilRenderer = sigils.GetComponent<Renderer>();
			Texture tex = sigilRenderer.material.mainTexture;
			sigilRenderer.material = Card.GetComponentInChildren<CardAbilityIcons>().emissiveIconMat;
			sigilRenderer.material.mainTexture = tex;
			sigilRenderer.material.SetColor(Color1, GameColors.Instance.glowSeafoam);

			//Handle Rotation
			int iterator = 0;
			foreach (Transform t in sigils.GetComponentsInChildren<Transform>())
			{
				t.localRotation = Quaternion.identity;
				t.localScale = icon.transform.GetComponentsInChildren<Transform>()[iterator].transform.localScale;
				t.localScale = new Vector3((t.localScale.x * 1.2f), (t.localScale.x * 1.2f), t.localScale.z);
				iterator++;
			}

			sigils.transform.rotation = Card.Slot.transform.rotation;
			sigils.transform.Rotate(Vector3.left, -90f);

			//Fix Rend
			List<CardAbilityIcons> icons = sigils.GetComponentsInParent<CardAbilityIcons>().ToList();
			icons.AddRange(sigils.GetComponentsInChildren<CardAbilityIcons>().ToList());
			foreach (var i in icons.Where(cai => cai.IsNotNull()))
			{
				DestroyImmediate(i);
			}

			sigils.SetActive(true);
			createdSigils.Add(sigils);
		}

		if (createdSigils.Count > 0)
		{
			Card.Slot.gameObject.AddComponent<HauntedSlot>()
				.Init(Card.Slot, createdSigils, Card.Info.Abilities.FindAll(x => x != Haunter.ability));
			GlobalTriggerHandler.Instance.RegisterNonCardReceiver(Card.Slot.GetComponent<HauntedSlot>());
		}

		yield break;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription = "When [creature] perishes, it haunts the space it died in. "
		                                   + "Creatures played on this space gain its old sigils.";

		return ApiUtils.CreateAbility<Haunter>(
			rulebookDescription,
			rulebookIcon: AbilitiesUtil.LoadAbilityIcon(Ability.Haunter.ToString())
		);
	}
}

public class HauntedSlot : NonCardTriggerReceiver
{
	[SerializeField] public CardSlot cardSlot;
	public float particleTimer = 0.1f;
	public bool setUp;
	public List<GameObject> hauntSigils;
	public List<Ability> abilities;

	public void Init(CardSlot slot, List<GameObject> visualSigils, List<Ability> actualSigils)
	{
		if (hauntSigils == null)
		{
			hauntSigils = new List<GameObject>();
		}

		if (abilities == null)
		{
			abilities = new List<Ability>();
		}

		hauntSigils.Clear();
		abilities.Clear();
		hauntSigils.AddRange(visualSigils);
		abilities.AddRange(actualSigils);
		setUp = true;
		this.cardSlot = slot;
	}

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		return setUp && otherCard.Slot == cardSlot;
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		otherCard.AddTemporaryMod(
			new CardModificationInfo { abilities = new List<Ability>(abilities), fromCardMerge = false, fromTotem = false }
		);
		otherCard.Anim.PlayTransformAnimation();
		otherCard.RenderCard();

		List<GameObject> haunts = hauntSigils;
		foreach (var haunt in haunts.Where(haunt => haunt.IsNotNull()))
		{
			CustomCoroutine.WaitThenExecute(0.1f, delegate() { Destroy(haunt); });
		}

		CustomCoroutine.WaitThenExecute(0.1f, delegate() { Destroy(this); });
		yield break;
	}

	public override void ManagedUpdate()
	{
		if (TurnManager.Instance == null || TurnManager.Instance.GameEnded)
		{
			foreach (GameObject haunt in hauntSigils)
			{
				Destroy(haunt);
			}

			Destroy(this);
		}

		foreach (GameObject floater in hauntSigils)
		{
			if (floater.GetComponent<HauntedSigilFloatData>().IsNotNull())
			{
				HauntedSigilFloatData floatData = floater.GetComponent<HauntedSigilFloatData>();
				if (floatData.movingUp)
				{
					float movement = 0.001f;
					if (floatData.currentHeight >= floatData.upperSlowThreshold
					    || floatData.currentHeight <= floatData.lowerSlowThreshold) movement *= 0.5f;
					if (floatData.currentHeight >= floatData.upperLimit)
					{
						movement *= -1;
						floatData.movingUp = false;
					}

					floater.transform.position += new Vector3(0, movement, 0);
					floatData.currentHeight = floater.transform.position.y;
				}
				else
				{
					float movement = -0.001f;
					if (floatData.currentHeight >= floatData.upperSlowThreshold
					    || floatData.currentHeight <= floatData.lowerSlowThreshold) movement *= 0.5f;
					if (floatData.currentHeight <= floatData.lowerLimit)
					{
						movement *= -1;
						floatData.movingUp = true;
					}

					floater.transform.position += new Vector3(0, movement, 0);
					floatData.currentHeight = floater.transform.position.y;
				}
				//icon.transform.position + new Vector3(0, 0.1f, 0)
			}
			else
			{
				HauntedSigilFloatData floatData = floater.AddComponent<HauntedSigilFloatData>();
				floatData.startHeight = floater.transform.position.y;

				floatData.lowerLimit = floater.transform.position.y - 0.09f;
				floatData.upperLimit = floater.transform.position.y + 0.16f;
				floatData.lowerSlowThreshold = floater.transform.position.y - 0.065f;
				floatData.upperSlowThreshold = floater.transform.position.y + 0.135f;

				//Random Start
				float randomMod = UnityEngine.Random.Range(-0.09f, 0.16f);
				float startY = floater.transform.position.y + randomMod;
				floatData.randomlySelectedFloatStart = startY;
				floater.transform.position += new Vector3(0, randomMod, 0);
				floatData.currentHeight = floater.transform.position.y;

				floatData.movingUp = UnityEngine.Random.value <= 0.5f;
			}
		}

		if (particleTimer >= 0)
		{
			particleTimer -= Time.deltaTime;
		}
		else
		{
			GameObject particles = Instantiate(
				SpecialNodeHandler.Instance.cardMerger.transformParticles.gameObject,
				cardSlot.transform.position,
				Quaternion.Euler(-90, 0, 0)
			);
			ParticleSystem.ShapeModule shape = particles.GetComponent<ParticleSystem>().shape;
			ParticleSystem.VelocityOverLifetimeModule vel = particles.GetComponent<ParticleSystem>().velocityOverLifetime;
			vel.enabled = false;
			particles.SetActive(false);
			particles.SetActive(true);
			particleTimer = UnityEngine.Random.Range(0.1f, 0.4f);
		}
	}
}

public class HauntedSigilFloatData : MonoBehaviour
{
	public float startHeight;
	public float randomlySelectedFloatStart;
	public float currentHeight;
	public bool movingUp;

	public float upperLimit;
	public float lowerLimit;
	public float lowerSlowThreshold;
	public float upperSlowThreshold;
}
