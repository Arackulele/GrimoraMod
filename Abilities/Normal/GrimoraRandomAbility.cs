using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraRandomAbility : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToDrawn() => true;

	public override IEnumerator OnDrawn()
	{
		(PlayerHand.Instance as PlayerHand3D).MoveCardAboveHand(Card);
		yield return Card.FlipInHand(AddMod);
		yield return LearnAbility(0.5f);
	}

	public override bool RespondsToResolveOnBoard() => !Card.Status.hiddenAbilities.Contains(Ability);

	public override IEnumerator OnResolveOnBoard()
	{
		Card.Anim.PlayTransformAnimation();
		AddMod();
		yield return new WaitForSeconds(0.5f);
		yield return LearnAbility(0.5f);
	}

		private void AddMod()
	{
		Card.Status.hiddenAbilities.Add(Ability);
		CardModificationInfo cardModificationInfo = new CardModificationInfo(ChooseAbility());
		CardModificationInfo cardModificationInfo2 = Card.TemporaryMods.Find(x => x.HasAbility(Ability)) 
		                                          ?? Card.Info.Mods.Find(x => x.HasAbility(Ability));

		if (cardModificationInfo2.IsNotNull())
		{
			cardModificationInfo.fromTotem = cardModificationInfo2.fromTotem;
			cardModificationInfo.fromCardMerge = cardModificationInfo2.fromCardMerge;
		}

		Card.AddTemporaryMod(cardModificationInfo);
	}

	public static readonly List<Ability> AbilitiesGrimoraRandomAbility = new List<Ability> {
		Ability.DrawRabbits,
		DrawSkeletonOnHit.ability,
		Ability.Strafe,
		Ability.Deathtouch,
		Ability.Evolve,
		Ability.Tutor,
		Ability.WhackAMole,
		Ability.DrawCopy,
		LooseLimb.ability,
		Ability.CorpseEater,
		Ability.QuadrupleBones,
		Ability.Submerge,
		Ability.DrawCopyOnDeath,
		Ability.Sharp,
		Ability.StrafePush,
		Ability.GuardDog,
		Ability.Flying,
		Ability.Reach,
		Ability.SplitStrike,
		Ability.TriStrike,
		Ability.IceCube,
		Ability.BoneDigger,
		Ability.AllStrike,
		Ability.BuffNeighbours,
		Ability.SkeletonStrafe,
		Ability.DrawNewHand,
		Ability.DeathShield,
		Ability.ExplodeOnDeath,
		Ability.Sentry,
		Ability.LatchDeathShield,
		Ability.LatchExplodeOnDeath,
		Ability.DoubleDeath,
		Ability.CreateBells,
		Ability.DrawRandomCardOnDeath,
		Ability.Loot,
		Ability.DebuffEnemy,
		Ability.SwapStats,
		Ability.DoubleStrike,
		Ability.MadeOfStone,
		Ability.GainAttackOnKill,
		AlternatingStrike.ability,
		AreaOfEffectStrike.ability,
		BloodGuzzler.ability,
		BoneThief.ability,
		BuffSkeletonsSeaShanty.ability,
		ChaosStrike.ability,
		ColdFront.ability,
		CreateArmyOfSkeletons.ability,
		CreateShipwrecks.ability,
		Erratic.ability,
		FlameStrafe.ability,
		HookLineAndSinker.ability,
		Imbued.ability,
		InvertedStrike.ability,
		LatchSubmerge.ability,
		LitFuse.ability,
		SpiritBearer.ability,
		Slasher.ability
	};

	private Ability ChooseAbility()
	{
		Ability randomAbility = Ability.RandomAbility;
		int num2 = 0;
		while (randomAbility == null)
		{
			while (randomAbility == Ability.RandomAbility)
			{
				randomAbility = AbilitiesGrimoraRandomAbility.GetRandomItem();
				num2++;
				if (num2 > 100)
				{
					return Ability.Sharp;
				}
			}
		}

		GrimoraPlugin.Log.LogDebug($"[GrimoraRandomAbility] Random ability chosen [{randomAbility}]");
		return randomAbility;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GrimoraRandomAbility()
	{
		const string rulebookDescription = "When [creature] is drawn, this sigil is replaced with another sigil at random.";
		AbilityBuilder<GrimoraRandomAbility>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.RandomAbility.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Random Ability")
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("random_pixel"))
		 .Build();
	}
}
