using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public class CumulativeTorment : AbilityBehaviour
{

	public static Ability ability;
	
	
	public override Ability Ability { get=>ability; }

	public override bool RespondsToAttackEnded()
	{
		return !this.Card.Dead;
	}

	public override IEnumerator OnAttackEnded()
	{
		yield return base.OnAttackEnded();
		var info = this.Card.Info;
		yield return this.Card.Die(false, this.Card);
		var changedInfo = Instantiate(this.Card.Info);
		changedInfo.name = info.name + " tormented";
		changedInfo.bonesCost++;
		changedInfo.baseAttack++;
		changedInfo.baseHealth++;
		yield return CardSpawner.Instance.SpawnCardToHand(changedInfo);
		if(info.name.Contains("tormented")) Destroy(info);
	}
}


public partial class GrimoraPlugin
{
	public void Add_Ability_CumulativeTorment()
	{
		const string rulebookDescription =
			"[creature] perishes after attacking, then a copy with +1 power and health that costs 1 more bone enters your hand. Resets after every battle.";

		AbilityBuilder<CumulativeTorment>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Cumulative Torment")
		 .Build();
	}
}
