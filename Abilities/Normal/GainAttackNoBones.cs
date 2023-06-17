using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class GainAttackNoBones : AbilityBehaviour
{
	public static Ability ability;

		public const string ModSingletonId = "GrimoraMod_Malnourishment";

	public override Ability Ability => ability;

	private CardModificationInfo _modInfo;

	private void Start()
	{
		_modInfo = new CardModificationInfo
		{
			singletonId = ModSingletonId
		};
		Card.AddTemporaryMod(_modInfo);
	}

	public override bool RespondsToUpkeep(bool playerUpkeep) => true;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		yield return PreSuccessfulTriggerSequence();
		if (ResourcesManager.Instance.PlayerBones < 1) _modInfo.attackAdjustment = 2;
		else _modInfo.attackAdjustment = 0;

		Card.OnStatsChanged();

		yield return new WaitForSeconds(0.25f);
		yield return LearnAbility(0.25f);

	}

}


public partial class GrimoraPlugin
{
	public void Add_Ability_GainAttackNoBones()
	{
		const string rulebookDescription =
			"If you have no Bones, at the start of your turn [creature] deals 2 more damage.";

		AbilityBuilder<GainAttackNoBones>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Bone Starved")
		 .Build();
	}
}
