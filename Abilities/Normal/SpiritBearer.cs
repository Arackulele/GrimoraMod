using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class SpiritBearer : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		yield return PreSuccessfulTriggerSequence();

		yield return new WaitForSeconds(0.2f);
		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.2f);
		yield return ResourcesManager.Instance.AddMaxEnergy(1);
		yield return ResourcesManager.Instance.AddEnergy(1);
		yield return new WaitForSeconds(0.3f);
		yield return LearnAbility(0.2f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_SpiritBearer()
	{
		const string rulebookDescription = "When [creature] is played, it provides an energy soul to its owner.";

		AbilityBuilder<SpiritBearer>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("spiritbearer_pixel"))
		 .Build();
	}
}
