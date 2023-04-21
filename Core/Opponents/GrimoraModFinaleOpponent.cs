using DiskCardGame;
using InscryptionAPI.Encounters;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModFinaleOpponent : Part1Opponent
{
	public static readonly OpponentManager.FullOpponent FullOpponent = OpponentManager.Add(
		GUID,
		"GrimoraModFinaleOpponent",
		GrimoraModBattleSequencer.FullSequencer.Id,
		typeof(GrimoraModFinaleOpponent)
	);

	public override string BlueprintSubfolderName => "Finale";

	public override void ModifyQueuedCard(PlayableCard card)
	{
		base.ModifyQueuedCard(card);
		AddCustomArmPrefabs(card);
	}

	public override void ModifySpawnedCard(PlayableCard card)
	{
		base.ModifySpawnedCard(card);
		AddCustomArmPrefabs(card);
	}

	private void AddCustomArmPrefabs(PlayableCard playableCard)
	{
		if(playableCard.GetComponent<GraveControllerExt>().SafeIsUnityNull())
		{
			GraveControllerExt.SetupNewController(playableCard.Anim as GravestoneCardAnimationController);
		}
		else
		{
			playableCard.GetComponent<GraveControllerExt>().HandleRotatingCustomArmsForOpponents(playableCard);
		}
	}
}
