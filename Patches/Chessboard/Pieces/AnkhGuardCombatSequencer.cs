using System.Collections;
using DiskCardGame;
using EasyFeedback.APIs;
using GracesGames.Common.Scripts;
using GrimoraMod.Saving;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class AnkhGuardCombatSequencer : GrimoraModBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
	GUID,
	nameof(AnkhGuardCombatSequencer),
	typeof(AnkhGuardCombatSequencer)
);

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		EncounterData data = new EncounterData
		{
			opponentType = Opponent.Type.Default,
			opponentTurnPlan = nodeData.blueprint
			 .turns.Select(bpList1 => bpList1.Select(bpList2 => bpList2.card).ToList())
			 .ToList()
		};
		data.opponentType = Opponent.Type.Default;

		return data;
	}

	//type 0: OnTurnEnd
	//type 1: OnCardPlayed
	//type 2: DamageAddedToScale
	//type 3: OnCardDie
	//type 4: On attack ended
	private static int Ruletype;

	//effect 0: 1 Damage gets added to your scale, one of your cards gains 1 attack
	//effect 1: 1 Damage gets added to your scale, one opponent card looses 1 attack
	//effect 2: Opponent Cards heal 1 Attack, you get 1 Bone
	//effect 3: Opponent Cards gain 1 Hp, you get 1 Soul
	//effect 4: All Cards in play get a random Sigil
	//effect 5: All Cards in play get 1 sigil removed
	//effect 6: A Zombie gets played on a random slot
	//effect 7: Opponent plays (the walkers, boneheap, skeleton or revenant)

	private static int RuleEffect;

	private IEnumerator Effect0()
	{

		Debug.Log("Triggering Effect0");

		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, true);

		List<CardSlot> slots = new(Singleton<BoardManager>.Instance.playerSlots);

		List<CardSlot> slotsfull = new List<CardSlot>();

		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			attackAdjustment = 1
		};

		foreach (var i in slots)
		{
			if (i.Card != null && !i.Card.Dead)
			{
				slotsfull.Add(i);
			}
		}



		if (slotsfull.Count > 0)
		{
			CardSlot chosenslot = slotsfull.GetRandomItem();
			if (chosenslot.Card != null && !chosenslot.Card.Dead)
			{
				chosenslot.Card.AddTemporaryMod(cardModificationInfo);
				chosenslot.Card.OnStatsChanged();
			}
		}

	}

	private IEnumerator Effect1()
	{

		Debug.Log("Triggering Effect1");

		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, true);

		List<CardSlot> slots = new(Singleton<BoardManager>.Instance.opponentSlots);

		List<CardSlot> slotsfull = new List<CardSlot>();

		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			attackAdjustment = -1
		};

		foreach (var i in slots)
		{
			if (i.Card != null && !i.Card.Dead)
			{
				slotsfull.Add(i);
			}
		}



		if (slotsfull.Count > 0)
		{
			CardSlot chosenslot = slotsfull.GetRandomItem();
			if (chosenslot.Card != null && !chosenslot.Card.Dead )
			{
				chosenslot.Card.AddTemporaryMod(cardModificationInfo);
				chosenslot.Card.OnStatsChanged();
			}
		}

	}

	private IEnumerator Effect2()
	{

		Debug.Log("Triggering Effect2");

		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards))
		{

			List<CardSlot> slots = new(Singleton<BoardManager>.Instance.opponentSlots);


			CardModificationInfo cardModificationInfo = new CardModificationInfo
			{
				attackAdjustment = 1
			};

			foreach (var i in slots)
			{
				if (i.Card != null && !i.Card.Dead)
				{
					i.Card.AddTemporaryMod(cardModificationInfo);
					i.Card.OnStatsChanged();
				}
			}

		}

		yield return ResourcesManager.Instance.AddBones(1);

		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Effect3()
	{

		Debug.Log("Triggering Effect3");

		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards))
		{
			List<CardSlot> slots = new(Singleton<BoardManager>.Instance.opponentSlots);


			CardModificationInfo cardModificationInfo = new CardModificationInfo
			{
				healthAdjustment = 1
			};

			foreach (var i in slots)
			{
				if (i.Card != null && !i.Card.Dead)
				{
					i.Card.AddTemporaryMod(cardModificationInfo);
					i.Card.OnStatsChanged();

				}
			}

		}

		yield return ResourcesManager.Instance.AddMaxEnergy(1);

		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Effect4()
	{
		List<CardSlot> slots;
		Debug.Log("Triggering Effect4");

		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) slots = new(Singleton<BoardManager>.Instance.AllSlots);
		else slots = new(Singleton<BoardManager>.Instance.playerSlots);
		Ability chosen = AbilitiesChosenByRule5.GetRandomItem();

		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			abilities = new List<Ability> { chosen }
		};

		foreach (var i in slots)
		{
			if (i.Card != null && i.Card.AllAbilities().Count < 5 && UnityEngine.Random.Range(0, 10) > 5)
			{
				i.Card.AddTemporaryMod(cardModificationInfo);
				i.Card.OnStatsChanged();
				i.Card.Anim.PlayTransformAnimation();
			}
		}

		yield return new WaitForSeconds(0.1f);

	}

	public static readonly List<Ability> AbilitiesChosenByRule5 = new List<Ability>
	{
		Ability.AllStrike,
		Ability.DoubleDeath,
		Ability.DoubleStrike,
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

	private IEnumerator Effect5()
	{

		Debug.Log("Triggering Effect5");

		List<CardSlot> slots;
		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) slots = new(Singleton<BoardManager>.Instance.AllSlots);
		else slots = new(Singleton<BoardManager>.Instance.opponentSlots);

		foreach (var i in slots)
		{
			if (i.Card!= null && !i.Card.Dead)
			{


				yield return i.Card.TakeDamage(1, null);


			}
		}

		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Effect6()
	{
		Debug.Log("Triggering Effect6");

		List<CardSlot> slots;
		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) slots = new(Singleton<BoardManager>.Instance.AllSlots);
		else slots = new(Singleton<BoardManager>.Instance.playerSlots);

		List<CardSlot> slotsempty = new List<CardSlot>();

		foreach (var i in slots)
		{
			if (i.Card == null)
			{
				slotsempty.Add(i);
			}
		}

		if (slotsempty != null && slotsempty.Count > 0) yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(NameZombie), slotsempty.GetRandomItem(), 0.25f, true);

	}

	private IEnumerator Effect7()
	{
		Debug.Log("Triggering Effect7");

		List<CardSlot> slots;
		if (!AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards)) slots = new(Singleton<BoardManager>.Instance.OpponentSlotsCopy);
		else slots = new(Singleton<BoardManager>.Instance.playerSlots);

		List<CardSlot> slotsempty = new List<CardSlot>();

		List<String> options = new List<String> { NameSkeleton, NameRevenant, NameFamily, NameBonepile };


		foreach (var i in slots)
		{
			if (i.Card == null)
			{
				slotsempty.Add(i);
			}
		}

		if (slotsempty.Count > 0) yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(options.GetRandomItem()), slotsempty.GetRandomItem(), 0.25f, true);

	}

	private IEnumerator DoEffect()
	{

		switch(RuleEffect)
		{
			case 0:
				yield return Effect0();
				break;
			case 1:
				yield return Effect1();
				break;
			case 2:
				yield return Effect2();
				break;
			case 3:
				yield return Effect3();
				break;
			case 4:
				yield return Effect4();
				break;
			case 5:
				yield return Effect5();
				break;
			case 6:
				yield return Effect6();
				break;
			case 7:
				yield return Effect7();
				break;

		}



	}


	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

		public override bool RespondsToResolveOnBoard() => true;

		public override IEnumerator PlayerUpkeep()
	{
		Debug.Log("Triggering PlayerUpkeep");
		if (Ruletype == 0) yield return DoEffect();

	}

		public override IEnumerator PlayerCombatPostAttacks()
	{
		Debug.Log("Triggering OnPlayFromHand");
		if (Ruletype == 1) yield return DoEffect();

	}
	int timer = 0;

	public override bool RespondsToOtherCardDie(
	PlayableCard card,
	CardSlot deathSlot,
	bool fromCombat,
	PlayableCard killer
)
	{
		return true;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		if (Ruletype == 2)
		{
			if (timer == 0) timer++;
			else
			{
				yield return DoEffect();
				timer = 0;
			}

		}
	}


		public override IEnumerator PreDeckSetup()
		{


			string triggerdesc = "none";

		string effectdesc = "none";

		Ruletype = UnityEngine.Random.Range(0, 3);

		RuleEffect = UnityEngine.Random.Range(0, 8);

		Debug.Log("Ankh Guard Trigger: " + Ruletype);

		Debug.Log("Ankh Guard Rule: " + RuleEffect);


		switch (Ruletype)
		{
			case 0:
				triggerdesc = "Every time your turn Starts, ";
				break;
			case 1:
				triggerdesc = "After your Cards attack, ";
				break;
			case 2:
				triggerdesc = "Every second Card that dies, ";
				break;
		}

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards))
		{
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.EasyGuards);

			switch (RuleEffect)
			{
				case 0:
					effectdesc = "1 of your Cards attack increases and i get no benefit";
					break;
				case 1:
					effectdesc = "1 of my Cards attack decreases  and i get no benefit";
					break;
				case 2:
					effectdesc = "You gain 1 bone and i get no benefit";
					break;
				case 3:
					effectdesc = "You gain 1 maximum soul and i get no benefit";
					break;
				case 4:
					effectdesc = "all of your cards gain a random sigil and i get no benefit";
					break;
				case 5:
					effectdesc = "all of my take 1 Damage and i get no benefit";
					break;
				case 6:
					effectdesc = "a zombie gets played in one of your slots and i get no benefit";
					break;
				case 7:
					effectdesc = "you play a card of many bones and i get no benefit";
					break;
			}
		}
		else { 
		switch (RuleEffect)
		{
			case 0:
				effectdesc = "1 Damage gets added to your side of the Scale and 1 of your Cards attack increases";
				break;
			case 1:
				effectdesc = "1 Damage gets added to your side of the Scale and 1 of my Cards attack decreases";
				break;
			case 2:
				effectdesc = "My Cards attack increases and you gain 1 bone";
				break;
			case 3:
				effectdesc = "My cards get healed and you gain 1 maximum soul";
				break;
			case 4:
				effectdesc = "all cards gain a random sigil";
				break;
			case 5:
				effectdesc = "all cards take 1 Damage";
				break;
			case 6:
				effectdesc = "a zombie gets played in a random slot";
				break;
			case 7:
				effectdesc = "i play a card of many bones";
				break;
		}
		}


		yield return TextDisplayer.Instance.ShowUntilInput($"THE GODS HAVE {"DECIDED".Gold()}!");

		yield return TextDisplayer.Instance.ShowUntilInput($" {triggerdesc.Gold() + effectdesc.Orange()}!");

	}


}
