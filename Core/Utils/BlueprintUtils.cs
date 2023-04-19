using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class BlueprintUtils
{
	#region BlueprintCardtranslation

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonehound = NameBonehound.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonelord = NameBonelord.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonepile = NameBonepile.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_BonePrince = NameBonePrince.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_PlagueDoctor = NamePlagueDoctor.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Summoner = NameSummoner.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadHand = NameDeadHand.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadPets = NameDeadPets.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Draugr = NameDraugr.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DrownedSoul = NameDrownedSoul.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Banshee = NameBanshee.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Family = NameFamily.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Flames = NameFlames.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_FrankAndStein = NameFranknstein.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_GhostShip = NameGhostShip.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Gravedigger = NameGravedigger.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_HeadlessHorseman = NameHeadlessHorseman.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Hydra = NameHydra.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Mummy = NameMummy.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Necromancer = NameNecromancer.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Obol = NameObol.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Poltergeist = NamePoltergeist.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Revenant = NameRevenant.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sarcophagus = NameSarcophagus.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_VengefulSpirit = NameVengefulSpirit.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skeleton = NameSkeleton.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skelemagus = NameSkelemagus.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sporedigger = NameSporedigger.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_TombRobber = NameTombRobber.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Zombie = NameZombie.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_BooHag = NameBooHag.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DanseMacabre = NameDanseMacabre.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Dybbuk = NameDybbuk.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Giant = NameGiant.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Project = NameProject.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Ripper = NameRipper.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_ScreamingSkull = NameScreamingSkull.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Silbon = NameSilbon.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Manananggal = NameManananggal.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Animator = NameAnimator.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Catacomb = NameCatacomb.CreateCardBlueprint(); //maybe can be used for a hell hound like fight

	public static readonly EncounterBlueprintData.CardBlueprint bp_Centurion = NameCenturion.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Dalgyal = NameDalgyal.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadTree = NameDeadTree.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeathKnell = NameDeathKnell.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_ForgottenMan = NameForgottenMan.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_MassGrave = NameMassGrave.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Gashadokuro = NameGashadokuro.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_GraveBard = NameGravebard.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Hellhand = NameHellhand.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_RotTail = NameRotTail.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_StarvedMan = NameStarvedMan.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_TamperedCoffin = NameTamperedCoffin.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Vampire = NameVampire.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Wechuge = NameWechuge.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Writher = NameWrither.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Floatsam = NameShipwreckDams.CreateCardBlueprint();

	//Cards not in blueprints cause they dont work for the enemy: Apparition, Calavera Catrina, Deadeye, Doll, Jikininki, Sluagh

	#region PiratesRoyal

	public static readonly EncounterBlueprintData.CardBlueprint bp_CaptainYellowbeard = NamePirateCaptainYellowbeard.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_FirstMateSnag = NamePirateFirstMateSnag.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Swashbuckler = NamePirateSwashbuckler.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Shipwreck = NameShipwreck.CreateCardBlueprint();

	public static readonly EncounterBlueprintData.CardBlueprint bp_Exploding_Pirate = NamePirateExploding.CreateCardBlueprint();


	#endregion

	#region Kaycee

	public static readonly EncounterBlueprintData.CardBlueprint bp_Glacier = NameGlacier.CreateCardBlueprint();

  #endregion

	#region Sawyer

	public static readonly EncounterBlueprintData.CardBlueprint bp_Kennel = NameKennel.CreateCardBlueprint();

	#endregion

	#endregion

	#region Dictionaries





	#region AnkhGuardBPs

	public static EncounterBlueprintData BuildAnkhGuardBPone()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Draugr_Summoner";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Ripper, bp_Ripper, bp_Ripper, bp_Ripper },
			new() { bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, bp_Draugr },
			new() { bp_Draugr },
			new() { bp_Summoner },
			new() { bp_Skeleton },
			new() { bp_Draugr, bp_Skeleton }
		};

		return blueprint;
	}

	#endregion


	internal static readonly Dictionary<Opponent.Type, List<EncounterBlueprintData>> RegionWithBlueprints = new()
	{
		{
			KayceeBossOpponent.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildKayceeRegionBlueprintOne(),
				BuildKayceeRegionBlueprintTwo(),
				BuildKayceeRegionBlueprintThree(),
				BuildKayceeRegionBlueprintFour(),
				BuildKayceeRegionBlueprintFive(),
				BuildKayceeRegionBlueprintSix(),
				BuildKayceeRegionBlueprintSeven(),
				BuildKayceeRegionBlueprintEight(),
				BuildKayceeRegionBlueprintNine()
			}
		},
		{
			SawyerBossOpponent.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildSawyerRegionBlueprintOne(),
				BuildSawyerRegionBlueprintTwo(),
				BuildSawyerRegionBlueprintThree(),
				BuildSawyerRegionBlueprintFour(),
				BuildSawyerRegionBlueprintFive(),
				BuildSawyerRegionBlueprintSix(),
				BuildSawyerRegionBlueprintSeven(),
				BuildSawyerRegionBlueprintEight(),
				BuildSawyerRegionBlueprintNine(),
			}
		},
		{
			RoyalBossOpponentExt.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildRoyalBossRegionBlueprintOne(),
				BuildRoyalBossRegionBlueprintTwo(),
				BuildRoyalBossRegionBlueprintThree(),
				BuildRoyalBossRegionBlueprintFour(),
				BuildRoyalBossRegionBlueprintFive(),
				BuildRoyalBossRegionBlueprintSix(),
				BuildRoyalBossRegionBlueprintSeven(),
				BuildRoyalBossRegionBlueprintEight(),
				BuildRoyalBossRegionBlueprintNine(),
			}
		},
		{
			GrimoraBossOpponentExt.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildGrimoraBossRegionBlueprintOne(),
				BuildGrimoraBossRegionBlueprintTwo(),
				BuildGrimoraBossRegionBlueprintThree(),
				BuildGrimoraBossRegionBlueprintFour(),
			}
		}
	};




	internal static readonly Dictionary<Opponent.Type, List<EncounterBlueprintData>> RegionWithBlueprintsHard = new()
	{
		{
			KayceeBossOpponent.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildKayceeRegionBlueprintOneHard(),
				BuildKayceeRegionBlueprintTwoHard(),
				BuildKayceeRegionBlueprintThreeHard(),
				BuildKayceeRegionBlueprintFourHard(),
				BuildKayceeRegionBlueprintFiveHard(),
				BuildKayceeRegionBlueprintSixHard(),
				BuildKayceeRegionBlueprintSevenHard(),
				BuildKayceeRegionBlueprintEightHard(),
				BuildKayceeRegionBlueprintNineHard()
			}
		},
		{
			SawyerBossOpponent.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildSawyerRegionBlueprintOne(),
				BuildSawyerRegionBlueprintTwo(),
				BuildSawyerRegionBlueprintThree(),
				BuildSawyerRegionBlueprintFour(),
				BuildSawyerRegionBlueprintFive(),
				BuildSawyerRegionBlueprintSix(),
				BuildSawyerRegionBlueprintSeven(),
				BuildSawyerRegionBlueprintEight(),
				BuildSawyerRegionBlueprintNine(),
			}
		},
		{
			RoyalBossOpponentExt.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildRoyalBossRegionBlueprintOne(),
				BuildRoyalBossRegionBlueprintTwo(),
				BuildRoyalBossRegionBlueprintThree(),
				BuildRoyalBossRegionBlueprintFour(),
				BuildRoyalBossRegionBlueprintFive(),
				BuildRoyalBossRegionBlueprintSix(),
				BuildRoyalBossRegionBlueprintSeven(),
				BuildRoyalBossRegionBlueprintEight(),
				BuildRoyalBossRegionBlueprintNine(),
			}
		},
		{
			GrimoraBossOpponentExt.FullOpponent.Id,
			new List<EncounterBlueprintData>
			{
				BuildGrimoraBossRegionBlueprintOne(),
				BuildGrimoraBossRegionBlueprintTwo(),
				BuildGrimoraBossRegionBlueprintThree(),
				BuildGrimoraBossRegionBlueprintFour(),
			}
		}
	};

	#endregion

	public static EncounterBlueprintData GetRandomBlueprintForRegion()
	{
		return RegionWithBlueprints.ElementAt(GrimoraRunState.CurrentRun.regionTier).Value.GetRandomItem();
	}

	public static EncounterBlueprintData GetRandomBlueprintForRegionHard()
	{
		return RegionWithBlueprintsHard.ElementAt(GrimoraRunState.CurrentRun.regionTier).Value.GetRandomItem();
	}

	public static EncounterBlueprintData BuildRandomBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = $"Random{UnityRandom.Range(1, 99999999)}_Blueprint";
		int numberOfTurns = UnityRandom.Range(3, 15);
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();
		for (int i = 0; i < numberOfTurns; i++)
		{
			int numberOfCardsThisTurn = UnityRandom.Range(0, 4);
			List<EncounterBlueprintData.CardBlueprint> cardBlueprints = new();
			for (int j = 0; j < numberOfCardsThisTurn; j++)
			{
				cardBlueprints.Add(AllPlayableGrimoraModCards.GetRandomItem().CreateBlueprint());
			}
			Log.LogDebug($"[Blueprints] Turn [{i}] [{cardBlueprints.Join(bp => bp.card.DisplayedNameEnglish)}]");
			blueprint.turns.Add(cardBlueprints);
		}
		Log.LogDebug("\n");

		return blueprint;
	}
	

	#region RegionBlueprints

	#region Kaycee

	internal static EncounterBlueprintData BuildKayceeBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Boss";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Zombie },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Draugr },
			new() { bp_Revenant },
			new() { bp_Skeleton, bp_Draugr },
			new(),
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new(),
			new() { bp_DrownedSoul }
		};

		return blueprint;
	}


	private static EncounterBlueprintData BuildKayceeRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Draugr_Summoner";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Summoner },
			new() { bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, bp_Draugr },
			new() { bp_Draugr },
			new() { bp_Summoner },
			new() { bp_Skeleton },
			new() { bp_Draugr, bp_Skeleton }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Draugr_Gravebard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GraveBard },
			new() { bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, bp_Draugr },
			new() { bp_Draugr },
			new() { bp_GraveBard },
			new() { bp_Skeleton },
			new() { bp_Draugr, bp_Skeleton }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Project_Sentry";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Dalgyal },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Dalgyal },
			new() { bp_Project },
			new(),
			new() { bp_Skeleton },
			new() { bp_Skeleton }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildKayceeRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Vampire_Sentry";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Dalgyal },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Skeleton },
			new() { bp_Vampire },
			new(),
			new() {  bp_Vampire },
			new() { bp_Skeleton }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Basic_Undead";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new(),
			new() { bp_Hellhand, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Skeleton },
			new(),
			new() { bp_FrankAndStein },
			new(),
			new() { bp_Zombie }
		};
		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintSix()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Glacier_Bait";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Revenant },
			new(),
			new() { bp_Glacier },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildKayceeRegionBlueprintSeven()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Glacier_Bait_Zombies";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_Glacier },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintEight()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Frozen_Ocean";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Floatsam },
			new() { bp_Draugr, bp_Floatsam },
			new(),
			new() { bp_Floatsam },
			new() { bp_Draugr },
			new() { bp_DrownedSoul },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintNine()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Flying_Undead";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new(),
			new() { bp_Banshee, bp_Banshee },
			new() { bp_Banshee },
			new() { bp_Skeleton },
			new(),
			new() { bp_FrankAndStein },
			new(),
			new() { bp_Banshee }
		};
		return blueprint;
	}
	#endregion

	#region KayceeHardMode


	private static EncounterBlueprintData BuildKayceeRegionBlueprintOneHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Draugr_Summoner_Hard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Summoner, bp_Draugr },
			new() { bp_Summoner },
			new() { bp_Zombie, bp_Skeleton, bp_Draugr },
			new() { bp_FrankAndStein },
			new() { bp_Summoner },
			new() { bp_Skeleton },
			new() { bp_Summoner, bp_Zombie },
			new() { bp_Summoner },
			new() { bp_Zombie },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintTwoHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Draugr_Gravebard_Hard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GraveBard },
			new() { bp_Glacier },
			new() { bp_Revenant , bp_Glacier },
			new() { bp_Zombie },
			new() { bp_GraveBard },
			new() { bp_GraveBard },
			new() { bp_Glacier }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintThreeHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Project_Sentry_Hard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Dalgyal },
			new() { bp_Ripper },
			new(),
			new() { bp_Dalgyal },
			new() { bp_Project },
			new(),
			new() { bp_Skeleton },
			new() { bp_Project },
			new() { bp_Ripper },
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildKayceeRegionBlueprintFourHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Vampire_Sentry_Hard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Vampire },
			new() { bp_Dalgyal, bp_Zombie },
			new(),
			new() { bp_Revenant },
			new() { bp_Vampire },
			new(),
			new() {  bp_Vampire },
			new() { bp_Gashadokuro, bp_Gashadokuro }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintFiveHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Basic_Undead_Hard";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Hellhand, bp_Hellhand },
			new() { bp_Writher },
			new() { bp_Hellhand, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Skeleton },
			new(),
			new() { bp_FrankAndStein },
			new(),
			new() { bp_Zombie }
		};
		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintSixHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Glacier_Bait";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Revenant },
			new(),
			new() { bp_Glacier },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildKayceeRegionBlueprintSevenHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Glacier_Bait_Zombies";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_Glacier },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintEightHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Frozen_Ocean";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Floatsam },
			new() { bp_Draugr, bp_Floatsam },
			new(),
			new() { bp_Floatsam },
			new() { bp_Draugr },
			new() { bp_DrownedSoul },
			new(),
			new() { bp_Skeleton, bp_Draugr },
			new() { bp_Glacier }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintNineHard()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Kaycee_Flying_Undead";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new(),
			new() { bp_Banshee, bp_Banshee },
			new() { bp_Banshee },
			new() { bp_Skeleton },
			new(),
			new() { bp_FrankAndStein },
			new(),
			new() { bp_Banshee }
		};
		return blueprint;
	}
	#endregion

	#region Royal

	internal static EncounterBlueprintData BuildRoyalBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Boss";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Summoner, bp_Zombie },
			new() { bp_Skeleton },
			new() { bp_BonePrince },
			new() { bp_Summoner },
			new() { bp_Skeleton },
			new() { bp_GhostShip },
			new(),
			new() { bp_Revenant },
			new() { bp_BonePrince },
			new() { bp_Revenant },
			new(),
			new(),
			new() { bp_GhostShip },
			new() { bp_BonePrince },
			new(),
			new() { bp_BonePrince },
			new(),
			new() { bp_Revenant }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Ships";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GhostShip, bp_Summoner },
			new(),
			new() { bp_ForgottenMan },
			new(),
			new() { bp_Summoner },
			new(),
			new(),
			new() { bp_GhostShip },
			new(),
			new() { bp_ForgottenMan }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Hellpirates";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() {  bp_Floatsam },
			new() {  bp_Manananggal },
			new() {  bp_Shipwreck },
			new(),
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_GhostShip }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Hellpirates_SilbonVar";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() {  bp_Floatsam },
			new() {  bp_Silbon },
			new() {  bp_Shipwreck },
			new(),
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_GhostShip }
		};

		return blueprint;

	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Mates";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_FirstMateSnag },
			new() { bp_Shipwreck },
			new() { bp_FirstMateSnag },
			new() { bp_Swashbuckler },
			new(),
			new() { bp_Shipwreck, bp_Swashbuckler },
			new() { bp_FirstMateSnag },
			new(),
			new() { bp_Swashbuckler },
			new(),
			new() { bp_Shipwreck }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Bellist";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton, bp_Skeleton, bp_Skeleton },
			new() { bp_DeathKnell },
			new(),
			new() { bp_Summoner },
			new(),
			new() { bp_Skeleton },
			new() { bp_DeathKnell },
			new() { bp_Skeleton , bp_Skeleton ,bp_Skeleton }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintSix()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Exploding_Barrels";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_FrankAndStein },
			new() { bp_Exploding_Pirate, bp_Exploding_Pirate, bp_Exploding_Pirate },
			new() { bp_Swashbuckler },
			new(),
			new() { bp_FrankAndStein },
			new() { bp_Exploding_Pirate, bp_Exploding_Pirate, bp_Exploding_Pirate },
			new(),
			new() { bp_Swashbuckler }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintSeven()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Captain";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_CaptainYellowbeard },
			new() { bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton, bp_Skeleton },
			new() ,
			new() { bp_CaptainYellowbeard },
			new(),
			new() { bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Skeleton }
		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintEight()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Drowned_Army";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_DrownedSoul },
			new() ,
			new() { bp_ForgottenMan },
			new() ,
			new() { bp_StarvedMan },
			new() { bp_DrownedSoul },
			new(),
			new() { bp_Skeleton },
			new() ,
			new() { bp_Skeleton },
			new() { bp_Skeleton }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintNine()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Royal_Deathbringers";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GhostShip, bp_Revenant },
			new() ,
			new() { bp_StarvedMan },
			new() ,
			new() { bp_Revenant },

		};

		return blueprint;
	}

	#endregion


	#region Grimora

	internal static EncounterBlueprintData BuildGrimoraBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Grimora_Boss";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Silbon },
			new() { bp_Obol },
			new() { bp_StarvedMan },
			new() ,
			new() { bp_GraveBard },
			new(),
			new() { bp_DanseMacabre },
			new(),
			new() { bp_DeathKnell },
			new(),
			new() { bp_Revenant },
			new(),
			new() { bp_Sarcophagus },
			new() { bp_Skelemagus }
		};
		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Grimora_Ancient_Army";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Obol,bp_Obol },
			new() { bp_BooHag, bp_BooHag },
			new() { bp_Hellhand },
			new(),
			new() { bp_Obol,bp_Obol },
			new() { bp_BooHag, bp_BooHag },
			new() ,
			new() { bp_Hellhand },
			new() { bp_Hellhand },
			new() { bp_BooHag }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Grimora_Bowling";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Mummy },
			new() { bp_Exploding_Pirate },
			new() { bp_Vampire },
			new() { bp_Mummy },
			new() { bp_Exploding_Pirate },
			new() { bp_Vampire },
			new() { bp_Vampire },
			new() { bp_Mummy },
			new() { bp_Exploding_Pirate },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Grimora_Macabre";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_DanseMacabre },
			new() { bp_ScreamingSkull },
			new(),
			new() { bp_Zombie, bp_Zombie, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new() { bp_Zombie, bp_Zombie, bp_Zombie },
			new(),
			new() { bp_DanseMacabre },
			new() { bp_ScreamingSkull },
			new(),
			new() { bp_Zombie, bp_Zombie, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new() { bp_Zombie, bp_Zombie, bp_Zombie }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Grimora_Impenetrable_Wall";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Dybbuk },
			new() { bp_Ripper },
			new() { bp_Dybbuk },
			new() { bp_Revenant },
			new() { bp_Dybbuk },
			new() { bp_Silbon },
			new(),
			new() { bp_Silbon }
		};

		return blueprint;
	}

	#endregion


	#region Sawyer

	internal static EncounterBlueprintData BuildSawyerBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Boss";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound, bp_Zombie },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Centurion },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Centurion },
			new(),
			new() { bp_Centurion },
			new() { bp_DeadPets, bp_DeadPets },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Target";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_MassGrave },
			new() { bp_Bonehound },
			new(),
			new(),
			new() { bp_MassGrave },
			new(),
			new() { bp_VengefulSpirit },
			new() { bp_VengefulSpirit },
			new() { bp_VengefulSpirit }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Target_Sarcophagus";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_Sarcophagus },
			new() { bp_Zombie },
			new(),
			new() { bp_VengefulSpirit },
			new(),
			new() { bp_VengefulSpirit },
			new() { bp_Sarcophagus },
			new() { bp_VengefulSpirit }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_DefenseLine";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Centurion },
			new() { bp_Obol },
			new() { bp_Obol },
			new() { bp_Centurion },
			new(),
			new(),
			new() { bp_Centurion },
			new() ,
			new() { bp_Obol },
			new() ,
			new(),
			new(),
			new() { bp_Centurion },
			new() ,
			new() { bp_Obol },
			new() { bp_Centurion }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Hell";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Hellhand },
			new() { bp_ScreamingSkull },
			new(),
			new() { bp_Hellhand },
			new() { bp_ScreamingSkull },
			new(),
			new(),
			new() { bp_Hellhand, bp_Hellhand },
			new() { bp_ScreamingSkull },
			new() ,
			new() ,
			new() ,
			new() { bp_ScreamingSkull, bp_Hellhand },
			new() { bp_ScreamingSkull }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Ripper_Rush";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new(),
			new() { bp_Ripper },
			new() { bp_Ripper },
			new() { bp_Zombie },
			new(),
			new(),
			new(),
			new(),
			new(),
			new() { bp_Ripper, bp_Ripper, bp_Ripper, bp_Ripper },
			new() { bp_Ripper, bp_Ripper, bp_Ripper, bp_Ripper },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintSix()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Spirit_Lines";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Poltergeist },
			new(),
			new() { bp_VengefulSpirit },
			new() { bp_VengefulSpirit, bp_VengefulSpirit },
			new() { bp_Poltergeist },
			new() { bp_VengefulSpirit, bp_VengefulSpirit },
			new() { bp_VengefulSpirit },
			new(),
			new() { bp_VengefulSpirit },
			new(),
			new() { bp_VengefulSpirit, bp_VengefulSpirit },

		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintSeven()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Spirit_Lines_Deadly";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_VengefulSpirit },
			new(),
			new() { bp_VengefulSpirit },
			new() { bp_Skelemagus, bp_VengefulSpirit },
			new() { bp_VengefulSpirit },
			new() { bp_VengefulSpirit, bp_VengefulSpirit },
			new() { bp_Skelemagus },
			new(),
			new() { bp_VengefulSpirit },
			new(),
			new() { bp_VengefulSpirit, bp_Skelemagus },

		};

		return blueprint;
	}
	private static EncounterBlueprintData BuildSawyerRegionBlueprintEight()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Hounds";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound },
			new(),
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Revenant },
			new() { bp_Bonehound },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Revenant },

		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintNine()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = "Sawyer_Wechuges";
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Wechuge },
			new(),
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Wechuge },
			new() { bp_Wechuge },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Wechuge },

		};

		return blueprint;
	}

	#endregion

	#endregion
}
