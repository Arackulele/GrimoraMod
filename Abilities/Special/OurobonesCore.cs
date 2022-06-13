using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class OurobonesCore : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		var info = Card.Info;
		var changedInfo = Instantiate(info);
		int additiveCost = (PlayableCard.HasAbility(Ability.Brittle) ? 1 : 2);
		changedInfo.bonesCost += additiveCost;
		changedInfo.baseAttack++;
		changedInfo.baseHealth++;
		changedInfo.Mods = new(info.Mods);
		yield return CardSpawner.Instance.SpawnCardToHand(changedInfo);
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_OurobonesCore()
	{
		ApiUtils.CreateSpecialAbility<OurobonesCore>();
	}
}
