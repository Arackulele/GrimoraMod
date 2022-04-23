using System.Collections;
using System.Text;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GraveControllerExt : GravestoneCardAnimationController
{
	private static readonly int Hover = Animator.StringToHash("hover");
	private static readonly int Hovering = Animator.StringToHash("hovering");

	private const string SkeletonArmsGiants = "SkeletonArms_Giants";
	private const string SkeletonArmsBase = "Skeleton2ArmsAttacks";
	private const string SkeletonArmsSentry = "Grimora_Sentry";

	private Animator _graveAnim;

	private PlayableCard _playableCard;

	private Animator _customArmBase;

	private Animator _customArmSentry;

	private Animator _customArmGiants;

	private bool IsGiant { get; set; }

	private void Start()
	{
		var oldController = GetComponent<GravestoneCardAnimationController>();
		CustomCoroutine.WaitThenExecute(0.1f, () => Destroy(oldController));
	}

	public void Setup(GravestoneCardAnimationController graveController)
	{
		cardRenderer = graveController.cardRenderer;
		intendedRendererYPos = cardRenderer.transform.localPosition.y;
		armAnim = graveController.armAnim;
		damageMarks = graveController.damageMarks;
		damageParticles = graveController.damageParticles;
		deathParticles = graveController.deathParticles;
		fadeMaterial = graveController.fadeMaterial;
		sacrificeMarker = graveController.sacrificeMarker;
		sacrificeHoveringMarker = graveController.sacrificeHoveringMarker;
		statsLater = graveController.statsLater;
		_graveAnim = graveController.Anim;

		if (graveController.PlayableCard)
		{
			_playableCard = graveController.PlayableCard;
			IsGiant = _playableCard.IsGrimoraGiant();
		}

		AddCustomArmPrefabs();
	}

	public Animator GetCustomArm(string animToPlay = "")
	{
		if (animToPlay.IsNotEmpty() && animToPlay == "attack_sentry")
		{
			return _customArmSentry;
		}

		return IsGiant ? _customArmGiants : _customArmBase;
	}

	public void PlaySpecificAttackAnimation(
		string animToPlay,
		bool attackPlayer,
		CardSlot targetSlot,
		Action impactCallback
	)
	{
		Log.LogDebug($"[PlaySpecificAttackAnimation] Playing [{animToPlay}]");
		DoingAttackAnimation = true;
		string typeToAttack = attackPlayer ? "attack_player" : "attack_creature";
		string soundId = "gravestone_card_" + typeToAttack;
		Animator customArm = GetCustomArm(animToPlay);
		customArm.gameObject.SetActive(true);
		customArm.Play(animToPlay, 0, 0);
		PlayAttackSound(soundId);
		impactKeyframeCallback = impactCallback;
	}

	public override void PlayAttackAnimation(bool attackPlayer, CardSlot targetSlot)
	{
		Animator customArmPrefab = GetCustomArm();

		armAnim.gameObject.SetActive(false);
		_graveAnim.Play("shake", 0, 0f);

		string typeToAttack = attackPlayer ? "attack_player" : "attack_creature";

		var animToPlay = GetAnimToPlay(typeToAttack, targetSlot);
		bool doPlayCustomAttack = animToPlay == "sniper_shoot" || animToPlay == "attack_middle_finger";

		if (doPlayCustomAttack || IsGiant)
		{
			Log.LogDebug($"Playing custom attack [{animToPlay}] for card {_playableCard.GetNameAndSlot()}");
			customArmPrefab.gameObject.SetActive(true);
			customArmPrefab.Play(animToPlay, 0, 0f);
			// if (animToPlay == "sniper_shoot")
			// {
			// 	CustomCoroutine.WaitThenExecute(
			// 		0.1f,
			// 		delegate
			// 		{
			// 			if (attackPlayer)
			// 			{
			// 				OnImpactAttackPlayerKeyframe();
			// 			}
			// 			else
			// 			{
			// 				OnImpactKeyframe();
			// 			}
			// 		});
			// }
		}
		else
		{
			Log.LogDebug($"Playing regular attack [{animToPlay}] for card {_playableCard.GetNameAndSlot()}");
			armAnim.gameObject.SetActive(true);
			armAnim.Play(animToPlay, 0, 0f);
		}

		string soundId = "gravestone_card_" + typeToAttack;
		PlayAttackSound(soundId);
	}

	private int GetNumToDetermineRotation(CardSlot targetSlot)
	{
		if (IsGiant)
		{
			// 0 < 1 for example
			return targetSlot.Index < _playableCard.Slot.Index ? -1 : 1;
		}

		return (targetSlot.Index - _playableCard.Slot.Index) * (_playableCard.Slot.IsPlayerSlot ? 1 : -1);
	}

	private string GetDirectionToAttack(int numberForRotation)
	{
		return numberForRotation switch
		{
			< 0 => "_left",
			> 0 => "_right",
			_   => string.Empty
		};
	}

	private string GetAnimToPlay(string typeToAttack, CardSlot targetSlot)
	{
		Log.LogDebug($"Checking Playable card {_playableCard.GetNameAndSlot()} TargetSlot {targetSlot} Attack == 0 ? [{_playableCard.Attack == 0}] Has sniper? [{_playableCard.HasAbility(Ability.Sniper)}]");

		if (_playableCard.Attack == 0)
		{
			return "attack_middle_finger";
		}

		if (_playableCard.HasAbility(Ability.Sniper))
		{
			return "sniper_shoot";
		}

		Log.LogDebug($"TargetSlotIdx [{targetSlot.Index}] Card Attacking idx [{_playableCard.Slot.Index}] is player? [{_playableCard.Slot.IsPlayerSlot}]");
		int numToDetermineRotation = GetNumToDetermineRotation(targetSlot);
		string directionToAttack = GetDirectionToAttack(numToDetermineRotation);
		Log.LogDebug($"Num to determine rotation [{numToDetermineRotation}] Direction To Attack [{directionToAttack}]");

		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;
		bool isCardOpponents = _playableCard.OpponentCard;
		bool hasAreaOfEffectStrike = _playableCard.HasAbility(AreaOfEffectStrike.ability);
		bool hasInvertedStrike = _playableCard.HasAbility(InvertedStrike.ability);
		bool targetSlotIsFarthestAway =
			Mathf.Abs(numToDetermineRotation) == BoardManager.Instance.PlayerSlotsCopy.Count - 1;

		bool cardIsTargetingAdjFriendly = isPlayerSideBeingAttacked && !isCardOpponents
		                               || !isPlayerSideBeingAttacked && isCardOpponents;

		StringBuilder animToPlay = new StringBuilder(typeToAttack + directionToAttack);

		if (IsGiant)
		{
			animToPlay.Append("_giant");
		}
		// else if (hasInvertedStrike)
		// {
		// 	if (targetSlotIsFarthestAway)
		// 	{
		// 		animToPlay.Append("_invertedstrike_far");
		// 	}
		// 	else if (Math.Abs(targetSlot.Index - playableCard.Slot.Index) == 2)
		// 	{
		// 		animToPlay.Append("_invertedstrike");
		// 	}
		// }
		else if (hasAreaOfEffectStrike || cardIsTargetingAdjFriendly)
		{
			if (isPlayerSideBeingAttacked)
			{
				if (!isCardOpponents)
				{
					animToPlay.Append("_adj");
				}
			}
			else
			{
				if (isCardOpponents)
				{
					animToPlay.Append("_adj");
				}
			}
		}

		return animToPlay.ToString();
	}

	public override IEnumerator FlipInAir()
	{
		_graveAnim.Play("card_flip_inair", 0, 0);
		yield break;
	}

	public override void PlayDeathAnimation(bool playSound = true)
	{
		if (UnityRandom.value < 0.33f || PlayableCard.IsNull())
		{
			PlayGlitchOutAnimation();
			return;
		}

		TweenShaderToBlack();

		Anim.Play("die", 0, 0f);
		PlayDeathParticles();
		if (playSound)
		{
			PlayDeathSound();
		}
	}

	private void TweenShaderToBlack()
	{
		Tween.ShaderColor(
			PlayableCard.StatsLayer.Material,
			"_FadeColor",
			Color.black,
			0.35f,
			0f,
			Tween.EaseIn,
			Tween.LoopType.None,
			null,
			delegate
			{
				PlayableCard.StatsLayer.Material = fadeMaterial;
				Tween.ShaderColor(
					PlayableCard.StatsLayer.Material,
					"_Color",
					new Color(0f, 0f, 0f, 0f),
					0.35f,
					0f,
					Tween.EaseLinear
				);
			}
		);
	}

	public override void PlayTransformAnimation()
	{
		_graveAnim.Play("card_flip", 0, 0);
	}

	public override void SetHovering(bool hovering)
	{
		bool isHovering = hovering || _graveAnim.GetBool(Hovering);
		if (isHovering)
		{
			_graveAnim.ResetTrigger(Hover);
			_graveAnim.SetTrigger(Hover);
		}

		_graveAnim.SetBool(Hovering, isHovering);
	}

	public void PlayAttackSound(string soundId)
	{
		AudioController.Instance.PlaySound3D(
			soundId,
			MixerGroup.TableObjectsSFX,
			transform.position,
			1f,
			0f,
			new AudioParams.Pitch(AudioParams.Pitch.Variation.Small),
			new AudioParams.Repetition(0.05f)
		);
	}

	public void PlayDeathSound()
	{
		AudioController.Instance.PlaySound3D(
			"card_death",
			MixerGroup.TableObjectsSFX,
			transform.position,
			1f,
			0f,
			new AudioParams.Pitch(AudioParams.Pitch.Variation.VerySmall),
			new AudioParams.Repetition(0.05f)
		);
	}

	private static readonly List<Ability> AbilitiesForSentryCustomArm = new()
	{
		Ability.Sentry, Ability.Sniper, ActivatedDealDamageGrimora.ability
	};

	private void AddCustomArmPrefabs()
	{
		if (_customArmBase.IsNull())
		{
			Animator customArmBase = Instantiate(AssetConstants.CustomSkeletonArmBase, transform).GetComponent<Animator>();
			customArmBase.runtimeAnimatorController = AssetConstants.SkeletonArmController;
			customArmBase.name = SkeletonArmsBase;
			customArmBase.gameObject.AddComponent<AnimMethods>();
			customArmBase.gameObject.SetActive(false);
			_customArmBase = customArmBase.GetComponent<Animator>();
		}

		if (AbilitiesForSentryCustomArm.Exists(Card.Info.HasAbility) && _customArmSentry.IsNull())
		{
			Log.LogDebug($"[AddCustomArmPrefabs] Spawning new sentry prefab for card [{Card.Info.displayedName}]");
			GameObject skeletonArmSentry = Instantiate(AssetConstants.CustomSkeletonArmSentry, transform);
			skeletonArmSentry.name = SkeletonArmsSentry;
			Animator animObj = skeletonArmSentry.transform.GetChild(0).gameObject.GetComponent<Animator>();
			animObj.runtimeAnimatorController = AssetConstants.SkeletonArmController;
			animObj.gameObject.AddComponent<AnimMethods>();
			animObj.gameObject.SetActive(false);
			_customArmSentry = animObj;
		}

		if (IsGiant && _customArmGiants.IsNull())
		{
			Log.LogDebug($"[AddCustomArmPrefabs] Adding skeleton arm giant prefab to card [{Card.Info.displayedName}]");
			Animator customArmGiants = Instantiate(AssetConstants.CustomSkeletonArmGiants, transform).GetComponent<Animator>();
			customArmGiants.runtimeAnimatorController = AssetConstants.SkeletonArmController;
			customArmGiants.name = SkeletonArmsGiants;
			customArmGiants.gameObject.AddComponent<AnimMethods>();
			customArmGiants.gameObject.SetActive(false);
			_customArmGiants = customArmGiants;
		}
	}
}
