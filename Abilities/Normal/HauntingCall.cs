using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using Sirenix.Utilities;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod;

public class HauntingCall : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return base.Card != null && base.Card.OpponentCard != playerUpkeep && !base.Card.Dead;
	}
	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		yield return base.PreSuccessfulTriggerSequence();

			if (ResourcesManager.Instance.PlayerBones == 0) yield return Card.Die(false);
			else
		{
			ViewManager.Instance.SwitchToView(View.BoneTokens, lockAfter: true);

			yield return new UnityEngine.WaitForSeconds(0.5f);
			yield return ResourcesManager.Instance.SpendBones(1);
			yield return new UnityEngine.WaitForSeconds(0.5f);
			ViewManager.Instance.SetViewUnlocked();
		}
		yield break;
	}

	public override bool RespondsToResolveOnBoard()
	{
		return base.Card != null && !base.Card.Dead && ResourcesManager.Instance.PlayerBones == 0;
	}
	public override IEnumerator OnResolveOnBoard()
	{
		yield return PreSuccessfulTriggerSequence();
		yield return Card.Die(false);
		yield return LearnAbility(0.25f);
		yield break;
	}

}

public partial class GrimoraPlugin
{
	public void Add_Ability_HauntingCall()
	{
		const string rulebookDescription =
			"Every turn [creature] is on the Board, it will take a Bone from you, if you have no Bones, it perishes.";

		AbilityBuilder<HauntingCall>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Haunting Call")
		 .Build();
	}
}
