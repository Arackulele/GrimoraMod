using DiskCardGame;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;

namespace GrimoraMod;

public class ElectricChairLever : HighlightedInteractable
{
	private static readonly Color DarkCellColor = new Color(0, 0.1f, 0.1f, 1f);

	private const float DefaultLeverDuration = 0.375f;

	public static List<Ability> AbilitiesSaferRisk = new()
	{
		Ability.DeathShield,
		Ability.DrawRabbits,
		Ability.DrawRandomCardOnDeath,
		Ability.Evolve,
		Ability.IceCube,
		Ability.OpponentBones,
		Ability.QuadrupleBones,
		Ability.Sentry,
		Ability.Sharp,
		BoneThief.ability,
		DrawSkeletonOnHit.ability,
		ColdFront.ability,
		LooseLimb.ability,
		SpiritBearer.ability,
		Ability.SteelTrap,
	};

	public static List<Ability> AbilitiesMinorRisk = new(AbilitiesSaferRisk)
	{
		Ability.LatchDeathShield,
		Ability.BoneDigger,
		Ability.BuffNeighbours,
		Ability.CreateBells,
		Ability.Deathtouch,
		Ability.DebuffEnemy,
		Ability.DrawCopyOnDeath,
		Ability.DrawNewHand,
		Ability.Flying,
		Ability.GainAttackOnKill,
		Ability.LatchBrittle,
		Ability.LatchExplodeOnDeath,
		Ability.Loot,
		Ability.MadeOfStone,
		Ability.MoveBeside,
		Ability.Reach,
		Ability.SkeletonStrafe,
		Slasher.ability,
		Ability.SplitStrike,
		Ability.Tutor,
		ActivatedDrawSkeletonGrimora.ability,
		ActivatedEnergyDrawWyvern.ability,
		ActivatedGainEnergySoulSucker.ability,
		AreaOfEffectStrike.ability,
		BuffSkeletonsSeaShanty.ability,
		ChaosStrike.ability,
		CreateArmyOfSkeletons.ability,
		CreateShipwrecks.ability,
		GrimoraRandomAbility.ability,
		HookLineAndSinker.ability,
		Imbued.ability
	};

	public static List<Ability> AbilitiesMajorRisk = new(AbilitiesMinorRisk)
	{
		LatchSubmerge.ability,
		Ability.ActivatedHeal,
		Ability.ActivatedRandomPowerEnergy,
		Ability.ActivatedStatsUp,
		Ability.ActivatedStatsUpEnergy,
		Ability.AllStrike,
		Ability.CorpseEater,
		Ability.DoubleDeath,
		Ability.DoubleStrike,
		Ability.DrawCopy,
		Ability.ExplodeOnDeath,
		Ability.GuardDog,
		Ability.Strafe,
		Ability.StrafePush,
		Ability.StrafeSwap,
		Ability.Submerge,
		Ability.SwapStats,
		Ability.TriStrike,
		Ability.WhackAMole,
		ActivatedDealDamageGrimora.ability,
		Anchored.ability,
		BloodGuzzler.ability,
		FlameStrafe.ability,
		Fylgja_GuardDog.ability,
		InvertedStrike.ability,
		MarchingDead.ability,
		Possessive.ability,
		Puppeteer.ability
	};

	public enum SigilRisk
	{
		Safe,
		Minor,
		Major
	}

	[SerializeField]
	public SigilRisk currentSigilRisk = SigilRisk.Safe;

	[SerializeField]
	private GameObject animHandle;

	[SerializeField]
	private CursorType pressCursorType = CursorType.Rotate;

	private Tuple<MeshRenderer, Color> _cellSaferRisk;
	private Tuple<MeshRenderer, Color> _cellMinorRisk;
	private Tuple<MeshRenderer, Color> _cellMajorRisk;

	private void Start()
	{

		animHandle = gameObject.transform.Find("Anim").gameObject;
		Transform cells = gameObject.transform.Find("Cells");
		// CetChild(i) EnergyCell -> Case -> Cell
		_cellSaferRisk = new Tuple<MeshRenderer, Color>(
			cells.GetChild(0).Find("Case").GetChild(0).GetComponent<MeshRenderer>(),
			new Color(0.1f, 0, 1)
		);
		_cellMinorRisk = new Tuple<MeshRenderer, Color>(
			cells.GetChild(1).Find("Case").GetChild(0).GetComponent<MeshRenderer>(),
			new Color(0.2f, 0, 0.5f)
		);
		_cellMajorRisk = new Tuple<MeshRenderer, Color>(
			cells.GetChild(2).Find("Case").GetChild(0).GetComponent<MeshRenderer>(),
			new Color(0.35f, 0, 0.5f)
		);

		SetCellColor(_cellMinorRisk.Item1, DarkCellColor);
		SetCellColor(_cellMajorRisk.Item1, DarkCellColor);

		HighlightCursorType = pressCursorType;

		CursorSelectStarted += ChangeRisk;
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.JammedChair))
		{
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.JammedChair);
			currentSigilRisk = SigilRisk.Major;
			SetCellColor(_cellMinorRisk.Item1, DarkCellColor);	
			SetCellColor(_cellSaferRisk.Item1, DarkCellColor);
			SetCellColor(_cellMajorRisk.Item1, _cellMajorRisk.Item2);
		}
	}

	public void ResetRisk()
	{
		currentSigilRisk = SigilRisk.Safe;
	}

	public Ability GetAbilityFromLeverRisk()
	{
		GrimoraPlugin.Log.LogDebug($"[GetAbilityFromLeverRisk] Current risk is [{currentSigilRisk}]");
		return currentSigilRisk switch
		{
			SigilRisk.Minor => AbilitiesMinorRisk.GetRandomItem(),
			SigilRisk.Major => AbilitiesMajorRisk.GetRandomItem(),
			_               => AbilitiesSaferRisk.GetRandomItem()
		};
	}

	private void ChangeRisk(MainInputInteractable i)
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.JammedChair))
		{
			AudioController.Instance.PlaySound3D("stone_object_short#1", MixerGroup.ExplorationSFX, this.gameObject.transform.position, 5f, 0f, new AudioParams.Pitch(0.5f + UnityEngine.Random.value * 0.05f), null, null, null, false);
			return;
		}
	switch (currentSigilRisk)
		{
			case SigilRisk.Safe:
				// -45 zed rotation to 0
				DoLeverAnimAndSetCurrentRisk(SigilRisk.Minor, 0, () => SetCellColor(_cellMinorRisk.Item1, _cellMinorRisk.Item2));
				break;
			case SigilRisk.Minor:
				// 0 zed rotation to 45
				DoLeverAnimAndSetCurrentRisk(SigilRisk.Major, 45, () => SetCellColor(_cellMajorRisk.Item1, _cellMajorRisk.Item2));
				break;
			case SigilRisk.Major:
			default:
				// 45 to -45 zed
				DoLeverAnimAndSetCurrentRisk(SigilRisk.Safe, -45, () =>
				{
					SetCellColor(_cellMinorRisk.Item1, DarkCellColor);
					SetCellColor(_cellMajorRisk.Item1, DarkCellColor);
				});
				break;
		}
	}

	private void DoLeverAnimAndSetCurrentRisk(SigilRisk riskToSetTo, int zedRotation, Action afterLeverAnimFinishes)
	{
		GrimoraPlugin.Log.LogDebug($"[ChangeRisk] current risk is [{currentSigilRisk}], setting to [{riskToSetTo}]");
		currentSigilRisk = riskToSetTo;
		TweenBase rotation = Tween.LocalRotation(animHandle.transform, Quaternion.Euler(0, 0, zedRotation), DefaultLeverDuration, 0.1f, Tween.EaseIn);
		CustomCoroutine.WaitOnConditionThenExecute(
			() => rotation.Status == Tween.TweenStatus.Finished,
			afterLeverAnimFinishes.Invoke
		);
	}

	private void SetCellColor(Renderer cell, Color c)
	{
		void FlickerOn()
		{
			cell.material.SetEmissionColor(c);
		}
		void FlickerOff()
		{
			cell.material.SetEmissionColor(DarkCellColor);
		}
		CustomCoroutine.FlickerSequence(FlickerOn, FlickerOff, false, true, 0.05f, 2);
	}
}
