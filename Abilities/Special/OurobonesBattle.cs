using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class OurobonesBattle : SpecialCardBehaviour
{
	public const string ModSingletonId = "GrimoraMod_Ourobones";
	
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	private CardModificationInfo _ouroMod;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		_ouroMod = PlayableCard.TemporaryMods.Find(mod => mod.singletonId == ModSingletonId);

		if (_ouroMod.IsNotNull())
		{
			_ouroMod.attackAdjustment++;
			_ouroMod.healthAdjustment++;
			_ouroMod.bonesCostAdjustment += 2;
		}
		else
		{
			_ouroMod = new CardModificationInfo(1, 1)
			{
				singletonId = ModSingletonId
			};
		}
		
		int additiveCost = (PlayableCard.HasAbility(Ability.Brittle) ? 1 : 2) * _ouroMod.attackAdjustment;
		CardInfo ouroInfo = AdjustBoneCost(additiveCost);
		yield return CardSpawner.Instance.SpawnCardToHand(ouroInfo, new List<CardModificationInfo> { _ouroMod });
	}
	
	private CardInfo AdjustBoneCost(int costAdjustment)
	{
		CardInfo ouroInfo = Card.Info.Clone() as CardInfo;
		ouroInfo.Mods.Add(new CardModificationInfo { bonesCostAdjustment = costAdjustment});
		return ouroInfo;
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_OurobonesBattle()
	{
		ApiUtils.CreateSpecialAbility<OurobonesBattle>();
	}
}
