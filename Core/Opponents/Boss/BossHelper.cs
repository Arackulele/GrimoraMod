using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public static class BossHelper
{
	public static readonly Dictionary<Opponent.Type, GameObject> BossMasksByType = new()
	{
		{ KayceeBossOpponent.FullOpponent.Id, AssetConstants.BossSkullKaycee },
		{ SawyerBossOpponent.FullOpponent.Id, AssetConstants.BossSkullSawyer },
		{ RoyalBossOpponentExt.FullOpponent.Id, AssetConstants.BossSkullRoyal }
	};

	public static Dictionary<string, Tuple<System.Type, GameObject, EncounterBlueprintData>>
		OpponentTupleBySpecialId = new()
		{
			{
				GrimoraModKayceeBossSequencer.FullSequencer.Id,
				new Tuple<System.Type, GameObject, EncounterBlueprintData>(
					GrimoraModKayceeBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceKaycee,
					BlueprintUtils.BuildKayceeBossInitialBlueprint()
				)
			},
			{
				GrimoraModSawyerBossSequencer.FullSequencer.Id,
				new Tuple<System.Type, GameObject, EncounterBlueprintData>(
					GrimoraModSawyerBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceSawyer,
					BlueprintUtils.BuildSawyerBossInitialBlueprint()
				)
			},
			{
				GrimoraModRoyalBossSequencer.FullSequencer.Id, new Tuple<System.Type, GameObject, EncounterBlueprintData>(
					GrimoraModRoyalBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceRoyal.gameObject,
					BlueprintUtils.BuildRoyalBossInitialBlueprint()
				)
			},
			{
				GrimoraModGrimoraBossSequencer.FullSequencer.Id,
				new Tuple<System.Type, GameObject, EncounterBlueprintData>(
					GrimoraModGrimoraBossSequencer.FullSequencer.SpecialSequencer,
					AssetConstants.BossPieceGrimora,
					BlueprintUtils.BuildGrimoraBossInitialBlueprint()
				)
			}
		};

}
