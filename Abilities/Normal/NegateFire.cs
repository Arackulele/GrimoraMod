using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class NegateFire : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		GrimoraPlugin.Log.LogDebug("Negating Fire");

		foreach (var i in BoardManager.Instance.AllSlots)
		{
			GrimoraPlugin.Log.LogDebug("Negating Fire2");
			if (i.Card != null)
			{
				GrimoraPlugin.Log.LogDebug("Negating Fire3");
				i.Card.AddTemporaryMod(new CardModificationInfo()
				{
					negateAbilities = new List<Ability>() { Burning.ability }
				});

				i.Card.OnStatsChanged();
				GameObject.Destroy(GameObject.Find("CardFire(Clone)"));
				yield return new WaitForSeconds(0.2f);
				GameObject.Destroy(GameObject.Find("CardFire(Clone)"));
			}
			
		}
		yield break;
	}


}



	public partial class GrimoraPlugin
	{
	public void Add_Ability_NegateFire()
	{

		const string rulebookDescription = "When played, [creature] will extinguish all cards on the Board.";

			AbilityBuilder<NegateFire>.Builder
			.SetRulebookName("Douse")
			 .SetRulebookDescription(rulebookDescription)
			 .Build();
		}
}

