using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CreateRoyalsCrewMate : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private int _timeToSpawnCounter = 0;

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return !playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		_timeToSpawnCounter++;
		var playerOpenSlots = BoardManager.Instance.GetPlayerOpenSlots();
		if (_timeToSpawnCounter >= 2 && playerOpenSlots.IsNotEmpty())
		{
			_timeToSpawnCounter = 0;

			yield return TextDisplayer.Instance.ShowUntilInput(
				$"PREPARE TO BE BOARDED, YAAAARRRRRR!"
			);
			yield return new WaitForSeconds(0.2f);

			yield return BoardManager.Instance.CreateCardInSlot(
				NamePirateSwashbuckler.GetCardInfo(),
				playerOpenSlots.GetRandomItem()
			);
		}
	}
	
	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] will spawn a Swashbuckler every 2 turns."
		                                   + $" [define:{NamePirateSwashbuckler}]";

		return ApiUtils.CreateAbility<CreateRoyalsCrewMate>(rulebookDescription, "Captain's Orders");
	}
}
