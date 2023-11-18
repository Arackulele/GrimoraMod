using System.Collections;
using DiskCardGame;
using EasyFeedback.APIs;
using GracesGames.Common.Scripts;
using GrimoraMod.Saving;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using JetBrains.Annotations;
using Steamworks;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using static GrimoraMod.GravestoneRenderStatsLayerPatches;
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

	//type 0: Sandstorm ( The rightmost Card on each Side will take 1 damage at the end of grimoras turn )
	//type 1: Heatwave ( burning, water urns )
	//type 2: Overgrowth ( dead tree is placed on a random space on both sides at the end of grimoras turn, dead trees will not spawn if there is only one open slot left on the side )
	//type 3: Witching Hour ( 1 slot one ach side becomes enchanted )
	//type 4: Witches Cauldron ( After attacking phase, 2 cards swap places )
	//type 5: Night of the living dead ( Energy/Bone cost swap )
	//type 6: Zombie Invasion ( every card becomes 1/1 for 2 bones )
	//type 7: Haunted Grounds ( every card except skeletons get haunter )
	//type 8: Great Flood ( middle and then outer slots become waterlogged )
	private static int CurrentEnv;


	private IEnumerator Effect0(bool forenemy)
	{

		Debug.Log("Triggering Effect0");
		List<CardSlot> validslots;

		if (!forenemy) validslots = Singleton<BoardManager>.Instance.playerSlots;
		else validslots = Singleton<BoardManager>.Instance.opponentSlots;

		float largestdistance = 0;

		PlayableCard targetselect = null;

			foreach (CardSlot slot in validslots)
			{
				if (slot && slot.Card)
				{
					float dist = Vector2.Distance(validslots.First().transform.position, slot.Card.transform.position);
					if (dist > largestdistance)
					{
						targetselect = slot.Card;
						largestdistance = dist;
					}
				}
			}

		if (targetselect != null && !targetselect.Dead) yield return targetselect.TakeDamage(1, null);

		yield break;
	}

	private IEnumerator Effect1(bool forenemy)
	{
		Debug.Log("Triggering Effect1");


		yield break;
	}

		private IEnumerator Effect2(bool forenemy)
	{
		Debug.Log("Triggering Effect2");
		bool safe = false;
		List<CardSlot> validslots;

		if (!forenemy) validslots = Singleton<BoardManager>.Instance.playerSlots;
		else validslots = Singleton<BoardManager>.Instance.opponentSlots;
		List<CardSlot> slotsempty = new List<CardSlot>();

			foreach (var i in validslots)
			{
				if (i.Card == null)
				{
					slotsempty.Add(i);
					safe = true;
				}
			}
		if (slotsempty.Count < 2) safe = false;
		if (safe == true) yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(NameDeadTree), slotsempty.GetRandomItem(), 0.25f, true);

		yield break;
	}

	CardSlot p;

	CardSlot e;

	private IEnumerator Effect3SetUp()
	{
		Debug.Log("Triggering Effect3Setup");

		p = Singleton<BoardManager>.Instance.playerSlots.GetRandomItem();
		yield return new WaitForSeconds(0.01f);
		e = Singleton<BoardManager>.Instance.opponentSlots.GetRandomItem();

		p.SetColors(GameColors.instance.purple, GameColors.instance.lightPurple, GameColors.instance.brightNearWhite);
		p.ShowState(p.currentState);
		e.SetColors(GameColors.instance.purple, GameColors.instance.lightPurple, GameColors.instance.brightNearWhite);
		e.ShowState(e.currentState);

		GameObject bewitchp = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("BewitchedSlot")));
		GameObject bewitche = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("BewitchedSlot")));

		bewitchp.transform.parent = p.transform;
		bewitchp.transform.localPosition = new Vector3(-0.0836f, 0, 0);

		bewitche.transform.parent = e.transform;
		bewitche.transform.localPosition = new Vector3(-0.0836f, 0, 0);

		yield break;
	}

	private IEnumerator Effect4(bool forenemy)
	{
		Debug.Log("Triggering Effect4");

		List<CardSlot> allslots;

		List<CardSlot> validslots;

		if (!forenemy)
		{
			allslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
			validslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
		}
		else
		{
			allslots = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
			validslots = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
		}

		foreach (var i in allslots)
		{
			if (i.Card == null || i.Card.HasAbility(Anchored.ability)) validslots.Remove(i);
		}

	  if(validslots.Count > 1)
		{
		PlayableCard swap1 = validslots.GetRandomItem().Card;
		validslots.Remove(swap1.Slot);
		yield return new WaitForSeconds(0.01f);
		PlayableCard swap2 = validslots.GetRandomItem().Card;

			if (swap1 != null && swap2 != null)
			{
				CardSlot temp = new CardSlot();
				CardSlot OldPos2 = swap2.Slot;
				CardSlot OldPos1 = swap1.Slot;
				yield return Singleton<BoardManager>.Instance.AssignCardToSlot(swap2, OldPos1);
				yield return Singleton<BoardManager>.Instance.AssignCardToSlot(swap1, OldPos2);
			}

		}

		yield break;
	}

	private IEnumerator Effect5(PlayableCard C)
	{
		Debug.Log("Triggering Effect5");

		int overflow = Math.Max(C.Info.BonesCost -6, 0);

		CardModificationInfo switchmod = new CardModificationInfo
		{
			bonesCostAdjustment = (C.Info.EnergyCost + overflow) - C.Info.BonesCost,
			energyCostAdjustment = Math.Min(C.Info.BonesCost - C.Info.EnergyCost, 6)
		};

		if (C.name != NameSkeleton)
		{
			C.AddTemporaryMod(switchmod);
			yield return new WaitForSeconds(0.05f);
			LiveUpdateEnergyDisplay(C);
		}

		

		yield break;
	}


	private IEnumerator Effect6(PlayableCard C)
	{
		Debug.Log("Triggering Effect6");

		CardModificationInfo zombiemod = new CardModificationInfo
		{
			attackAdjustment = -(C.Attack - 1),
			healthAdjustment = -(C.Health - 1),
			bonesCostAdjustment = -(C.Info.BonesCost - 2),
			energyCostAdjustment = -(C.Info.EnergyCost)
		};

		if (C.name != NameSkeleton)
		{
			C.AddTemporaryMod(zombiemod);
			yield return new WaitForSeconds(0.05f);
			LiveUpdateEnergyDisplay(C);
		}

		yield break;
	}

	private IEnumerator Effect7(PlayableCard C)
	{
		Debug.Log("Triggering Effect7");

		CardModificationInfo hauntedmod = new CardModificationInfo
		{
			abilities = new List<Ability>() { Haunter.ability }
		};

		if (C.LacksAbility(Ability.Brittle) && C != null && !C.Dead) C.AddTemporaryMod(hauntedmod);

		yield break;
	}

	public List<CardSlot> CurrentWaterlog = new List<CardSlot>();

	int cycle = 0;

	private IEnumerator Effect8()
	{
		Debug.Log("Triggering Effect8");

		if (cycle < 2)
		{
			cycle++;
			CurrentWaterlog = new List<CardSlot>();
		}
		else if (cycle < 4)
		{
			cycle++;
			CurrentWaterlog = new List<CardSlot>() {
			Singleton<BoardManager>.Instance.OpponentSlotsCopy.First(), Singleton<BoardManager>.Instance.OpponentSlotsCopy.Last(),
			Singleton<BoardManager>.Instance.PlayerSlotsCopy.First(), Singleton<BoardManager>.Instance.PlayerSlotsCopy.Last()
		};
		}
		else if (cycle < 6)
		{
			cycle++;
			CurrentWaterlog = new List<CardSlot>() {
			Singleton<BoardManager>.Instance.OpponentSlotsCopy[1], Singleton<BoardManager>.Instance.OpponentSlotsCopy[2],
			Singleton<BoardManager>.Instance.PlayerSlotsCopy[1], Singleton<BoardManager>.Instance.PlayerSlotsCopy[2]
		};
		}
		else cycle = 0;

		foreach (var i in CurrentWaterlog) 
		{

		i.SetColors(GameColors.instance.blue, GameColors.instance.brightBlue, GameColors.instance.brightNearWhite);

			if (i.Card != null && CurrentWaterlog.Contains(i) && !i.Card.HasAbility(Ability.Submerge) && !i.Card.Dead && !i.Card.HasAbility(Ability.MadeOfStone) && !i.Card.HasAbility(Ability.Haunter))
			{
				StartCoroutine(killwithdelay(i.Card));
			}
		}


		foreach (var i in BoardManager.Instance.AllSlots)
		{

			if (!CurrentWaterlog.Contains(i)) i.ResetColors();

		}


		yield break;
	}


	public IEnumerator killwithdelay(PlayableCard i)
	{
		yield return new WaitForSeconds(0.01f);
		yield return i.Die(false, null);
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
				return true;
		}

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
				return true;
		}

		public override bool RespondsToOtherCardDrawn(PlayableCard card)
		{
		return true;
		}

		public override IEnumerator OpponentCombatEnd()
		{
		if (CurrentEnv == 8) yield return Effect8();
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
		if (playerTurnEnd && CurrentEnv == 0) yield return Effect0(false);
		else if (CurrentEnv == 0) yield return Effect0(true);

		if (playerTurnEnd && CurrentEnv == 2) yield return Effect2(false);
		else if (CurrentEnv == 2) yield return Effect2(true);

		if (playerTurnEnd && CurrentEnv == 4) yield return Effect4(false);
		else if (CurrentEnv == 4) yield return Effect4(true);

		yield break;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
		CardModificationInfo statbuffwitched = new CardModificationInfo
		{
			attackAdjustment = 1,
			healthAdjustment = 1,
		};

		if (otherCard.slot == p || otherCard.slot == e)
		{



			yield return new WaitForSeconds(0.2f);

			otherCard.AddTemporaryMod(statbuffwitched);
			otherCard.OnStatsChanged();
		}
		else otherCard.RemoveTemporaryMod(statbuffwitched);

		CardModificationInfo zombiemod = new CardModificationInfo
		{
			attackAdjustment = -(otherCard.Attack - 1),
			healthAdjustment = -(otherCard.Health - 1),
			bonesCostAdjustment = -(otherCard.Info.BonesCost - 2),
			energyCostAdjustment = -(otherCard.Info.EnergyCost)
		};

		if (CurrentEnv == 1 && otherCard != null && !otherCard.Dead) otherCard.AddTemporaryMod(new CardModificationInfo(Burning.ability));

		if (CurrentEnv == 7 && !otherCard.HasAbility(Ability.Brittle) && !otherCard.HasAbility(Haunter.ability) && !otherCard.Dead ) otherCard.AddTemporaryMod(new CardModificationInfo(Haunter.ability));

		if (CurrentEnv == 6 && !otherCard.HasAbility(Ability.Brittle) && !otherCard.Dead) otherCard.AddTemporaryMod(zombiemod);

		if ( CurrentWaterlog.Contains(otherCard.slot) && !otherCard.HasAbility(Ability.Submerge) && !otherCard.Dead && !otherCard.HasAbility(Ability.MadeOfStone)) yield return otherCard.Die(false, null);

	}

	public override IEnumerator OnOtherCardDrawn(PlayableCard card)
	{
		if (CurrentEnv == 5 && !card.HasAbility(Ability.Brittle)) yield return Effect5(card);
		if (CurrentEnv == 6 && !card.HasAbility(Ability.Brittle)) yield return Effect6(card);
	}



	public override IEnumerator PreDeckSetup()
		{
		string triggerdesc;
		string triggertitle;

		//TODO: Seeded Random
		CurrentEnv = UnityEngine.Random.Range(0, 9);

		switch (CurrentEnv)
			{
			default:
			case 0:
				yield return TextDisplayer.Instance.ShowUntilInput($"A {"SANDSTORM".Gold()} IS BREWING!");
				triggerdesc = "At the end of a turn, the rightmost Card will take 1 Damage!";
				break;
			case 1:
				yield return TextDisplayer.Instance.ShowUntilInput($"ITS GETTING HOTTER!A {"HEATWAVE".Gold()} IS STARTING!");
				triggerdesc = "All cards on the board will burn!";
				break;
			case 2:
				yield return TextDisplayer.Instance.ShowUntilInput($"TREES START SPROUTING AROUND YOU.AN {"OVERGROWTH".Gold()} IS INBOUND!");
				triggerdesc = "Trees will sprout every turn!";
				break;
			case 3:
				yield return TextDisplayer.Instance.ShowUntilInput($"YOU HEAR WITCHES CHANTING.THE CLOCK STRIKES THE {"WITCHING HOUR".Gold()}!");
				triggerdesc = "Cards on enchanted slots will gain more attack and health!";
				break;
			case 4:
				yield return TextDisplayer.Instance.ShowUntilInput($"A CAULDRON IS BUBBLING IN THE DISTANCE.YOUR CARDS ARE PUT IN THE {"WITCHES CAULDRON".Gold()}!");
				triggerdesc = "2 Cards will swap after the end of the turn!";
				break;
			case 5:
				yield return TextDisplayer.Instance.ShowUntilInput($"SKELETONS START DANCING IN THE DISTANCE, YOU CAN SEE GHOSTS MOVE AROUND.ITS THE {"NIGHT OF THE LIVING DEAD".Gold()}!");
				triggerdesc = "Bone and soul cost of all cards are swapped!";
				break;
			case 6:
				yield return TextDisplayer.Instance.ShowUntilInput($"ZOMBIES ARE APPROACHING FROM ALL SIDES.A {"ZOMBIE INVASION".Gold()} WILL APPROACH!");
				triggerdesc = "Every card is now a Zombie!";
				break;
			case 7:
				yield return TextDisplayer.Instance.ShowUntilInput($"THE DIRT IS PLAGUED WITH SPIRITS FROM CENTURIES PAST.YOU'VE APPROACHED THE {"HAUNTED GROUNDS".Gold()}!");
				triggerdesc = "Every non Skeletal Card will gain Haunter!";
				break;
			case 8:
				yield return TextDisplayer.Instance.ShowUntilInput($"THE WATER LEVEL ALL AROUND IS RISING SLOWLY.A {"GREAT FLOOD".Gold()} IS STARTING!");
				triggerdesc = "Different slots on the board will become waterlogged, non waterborne cards on these slots will drown!";
				break;


		}

		yield return TextDisplayer.Instance.ShowUntilInput($" {triggerdesc.Gold()}");

		if (CurrentEnv == 3) yield return Effect3SetUp();

		if (CurrentEnv == 8) cycle++;

		if (CurrentEnv == 1)
		{
			yield return CardSpawner.Instance.SpawnCardToHand(NameUrn.GetCardInfo());
			yield return CardSpawner.Instance.SpawnCardToHand(NameUrn.GetCardInfo());
			yield return CardSpawner.Instance.SpawnCardToHand(NameUrn.GetCardInfo());
		}

		if (SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.EasyGuards))
		{
			yield return ResourcesManager.Instance.AddBones(1);
			yield return ResourcesManager.Instance.AddMaxEnergy(1);
			yield return ResourcesManager.Instance.AddEnergy(1);

		}

	}

	public override IEnumerator GameEnd(bool playerWon)
		{
		foreach (var i in BoardManager.Instance.AllSlots)
		{

			i.ResetColors();

			GameObject.Destroy(GameObject.Find("BewitchedSlot(Clone)"));
			yield return new WaitForSeconds(0.05f);

		}

		yield return base.GameEnd(playerWon);
		}

		private void LiveUpdateEnergyDisplay(Card C)
		{
		if (C != null) { 
			foreach (Transform transform in C.transform)
			{
				if (transform.name != "RotatingParent") transform.gameObject.SetActive(false);
			}

		GrimoraPlugin.Log.LogInfo(" checking for Energy availability");
		if (C.Info.EnergyCost >= 1)
		{
			GrimoraPlugin.Log.LogInfo("Calculating Energy Stuff");
			GrimoraPlugin.Log.LogInfo("energy: " + C.Info.EnergyCost);

			GrimoraPlugin.Log.LogInfo("adding Energy");
			if (C.Info.EnergyCost >= 1 && C.transform.Find("Energy1") != null)
			{
				C.transform.Find("Energy1").gameObject.SetActive(true);
			}
			if (C.Info.EnergyCost >= 2 && C.transform.Find("Energy2") != null)
				{
				C.transform.Find("Energy2").gameObject.SetActive(true);
			}
			if (C.Info.EnergyCost >= 3 && C.transform.Find("Energy3") != null)
				{
				C.transform.Find("Energy3").gameObject.SetActive(true);
			}
			if (C.Info.EnergyCost >= 4 && C.transform.Find("Energy4") != null)
				{
				C.transform.Find("Energy4").gameObject.SetActive(true);
			}
			if (C.Info.EnergyCost >= 5 && C.transform.Find("Energy5") != null)
				{
				C.transform.Find("Energy5").gameObject.SetActive(true);
			}
			if (C.Info.EnergyCost > 5 && C.transform.Find("Energy6") != null)
				{
				C.transform.Find("Energy6").gameObject.SetActive(true);
			}

		}


		}

	}


}
