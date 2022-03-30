using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class SoulSucker : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public int kills = 0;

	public Action doUpdateKillCountAndRerenderCard;

	public override bool RespondsToOtherCardResolve(PlayableCard otherCard) 
		=> !otherCard.OpponentCard && doUpdateKillCountAndRerenderCard != null;

	public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
	{
		doUpdateKillCountAndRerenderCard.Invoke();
		doUpdateKillCountAndRerenderCard = null;
		yield break;
	}

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		return fromCombat && killer == Card;
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		if(kills++ < 4)
		{
			UpdateKillCountAndRerenderCard(kills);
		}
		Card.Anim.StrongNegationEffect();
		yield break;
	}

	public void UpdateKillCountAndRerenderCard(int setKillsTo)
	{
		ViewManager.Instance.SwitchToView(View.Board);
		kills = setKillsTo;
		Card.Anim.StrongNegationEffect();
		string iconNumber = setKillsTo == 0 ? "ability_SoulSucker" : $"ability_SoulSucker_{kills}";
		Card.RenderInfo.OverrideAbilityIcon(ability, AssetUtils.GetPrefab<Texture>(iconNumber));
		Card.RenderCard();
	}

	public int UseSoulsAndReturnEnergyToAdd(int energyDiff)
	{
		GrimoraPlugin.Log.LogDebug($"[SoulSucker] {Card.GetNameAndSlot()} Current Kills [{kills}] EnergyDiff [{energyDiff}]");
		// assume energy diff is 3
		// kills is 2
		
		// 3 - 2 == 1
		int newEnergyDiff = energyDiff - kills;
		int energyToAdd = 0;
		if (kills > 0)
		{
			// 2 - 3 == -1 
			// 4 - 3 == 1 
			int killsLeft = Math.Max(0, kills - energyDiff);
			energyToAdd = newEnergyDiff <= 0 ? energyDiff : kills;
			// newEnergyDiff abs == 1
			// max(1, 4) == 4
			GrimoraPlugin.Log.LogDebug($"[SoulSucker] EnergyToAdd [{energyToAdd}] Kills Left [{killsLeft}]");
			doUpdateKillCountAndRerenderCard = () => UpdateKillCountAndRerenderCard(killsLeft);
		}
		else
		{
			doUpdateKillCountAndRerenderCard = null;
		}

		return energyToAdd;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_SoulSucker()
	{
		const string rulebookDescription =
			"When [creature] kills another creature, their soul will be drained. This card retains up to 4 souls." +
			"This card's soul count will be used to cover any missing energy.";

		ApiUtils.CreateAbility<SoulSucker>(rulebookDescription);
	}
}
