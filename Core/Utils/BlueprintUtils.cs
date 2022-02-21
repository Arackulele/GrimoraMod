using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class BlueprintUtils
{
	#region CardBlueprints

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonehound = new()
	{
		card = CardLoader.GetCardByName(NameBonehound)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonelord = new()
	{
		card = CardLoader.GetCardByName(NameBonelord)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonepile = new()
	{
		card = CardLoader.GetCardByName(NameBonepile)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_BonePrince = new()
	{
		card = CardLoader.GetCardByName(NameBonePrince)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_PlagueDoctor = new()
	{
		card = CardLoader.GetCardByName(NamePlagueDoctor)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_ArmoredZombie = new()
	{
		card = CardLoader.GetCardByName(NameArmoredZombie)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Summoner = new()
	{
		card = CardLoader.GetCardByName(NameSummoner)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadHand = new()
	{
		card = CardLoader.GetCardByName(NameDeadHand)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadPets = new()
	{
		card = CardLoader.GetCardByName(NameDeadPets)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Draugr = new()
	{
		card = CardLoader.GetCardByName(NameDraugr)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DrownedSoul = new()
	{
		card = CardLoader.GetCardByName(NameDrownedSoul)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_EmberSpirit = new()
	{
		card = CardLoader.GetCardByName(NameEmberSpirit)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Family = new()
	{
		card = CardLoader.GetCardByName(NameFamily)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Flames = new()
	{
		card = CardLoader.GetCardByName(NameFlames)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_FrankAndStein = new()
	{
		card = CardLoader.GetCardByName(NameFranknstein)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_GhostShip = new()
	{
		card = CardLoader.GetCardByName(NameGhostShip)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Gravedigger = new()
	{
		card = CardLoader.GetCardByName(NameGravedigger)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_HeadlessHorseman = new()
	{
		card = CardLoader.GetCardByName(NameHeadlessHorseman)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Hydra = new()
	{
		card = CardLoader.GetCardByName(NameHydra)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_MudWorm = new()
	{
		card = CardLoader.GetCardByName(NameMudWorm)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Mummy = new()
	{
		card = CardLoader.GetCardByName(NameMummy)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Necromancer = new()
	{
		card = CardLoader.GetCardByName(NameNecromancer)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Obol = new()
	{
		card = CardLoader.GetCardByName(NameObol)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Poltergeist = new()
	{
		card = CardLoader.GetCardByName(NamePoltergeist)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Revenant = new()
	{
		card = CardLoader.GetCardByName(NameRevenant)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sarcophagus = new()
	{
		card = CardLoader.GetCardByName(NameSarcophagus)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skelemancer = new()
	{
		card = CardLoader.GetCardByName(NameVengefulSpirit)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skeleton = new()
	{
		card = CardLoader.GetCardByName(NameSkeleton)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_SkeletonMage = new()
	{
		card = CardLoader.GetCardByName(NameSkelemagus)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sporedigger = new()
	{
		card = CardLoader.GetCardByName(NameSporedigger)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_TombRobber = new()
	{
		card = CardLoader.GetCardByName(NameTombRobber)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Zombie = new()
	{
		card = CardLoader.GetCardByName(NameZombie)
	};

	//V2.5 Cards

	public static readonly EncounterBlueprintData.CardBlueprint bp_BooHag = new()
	{
		card = CardLoader.GetCardByName(NameBooHag)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DanseMacabre = new()
	{
		card = CardLoader.GetCardByName(NameDanseMacabre)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Dybbuk = new()
	{
		card = CardLoader.GetCardByName(NameDybbuk)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Giant = new()
	{
		card = CardLoader.GetCardByName(NameGiant)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Project = new()
	{
		card = CardLoader.GetCardByName(NameProject)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Ripper = new()
	{
		card = CardLoader.GetCardByName(NameRipper)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_ScreamingSkull = new()
	{
		card = CardLoader.GetCardByName(NameScreamingSkull)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Silbon = new()
	{
		card = CardLoader.GetCardByName(NameSilbon)
	};
	#endregion

	internal static readonly Dictionary<Opponent.Type, List<EncounterBlueprintData>> RegionWithBlueprints = new()
	{
		{
			BaseBossExt.KayceeOpponent,
			new List<EncounterBlueprintData>()
			{
				BuildKayceeRegionBlueprintOne(),
				BuildKayceeRegionBlueprintTwo(),
				BuildKayceeRegionBlueprintThree(),
				BuildKayceeRegionBlueprintFour(),
				BuildKayceeRegionBlueprintFive()
			}
		},
		{
			BaseBossExt.SawyerOpponent,
			new List<EncounterBlueprintData>()
			{
				BuildSawyerRegionBlueprintOne(),
				BuildSawyerRegionBlueprintTwo(),
				BuildSawyerRegionBlueprintThree(),
				BuildSawyerRegionBlueprintFour(),
				BuildSawyerRegionBlueprintFive()
			}
		},
		{
			BaseBossExt.RoyalOpponent,
			new List<EncounterBlueprintData>()
			{
				BuildRoyalBossRegionBlueprintOne(),
				BuildRoyalBossRegionBlueprintTwo(),
				BuildRoyalBossRegionBlueprintThree(),
				BuildRoyalBossRegionBlueprintFour(),
				BuildRoyalBossRegionBlueprintFive()
			}
		},
		{
			BaseBossExt.GrimoraOpponent,
			new List<EncounterBlueprintData>()
			{
				BuildGrimoraBossRegionBlueprintOne(),
				BuildGrimoraBossRegionBlueprintTwo(),
				BuildGrimoraBossRegionBlueprintThree(),
				BuildGrimoraBossRegionBlueprintFour(),
			}
		}
	};


	#region RegionBlueprints

	private static EncounterBlueprintData BuildGeneralRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_BonePrince, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_GhostShip },
			new(),
			new() { bp_BonePrince },
			new() { bp_Zombie }
		};

		return blueprint;
	}

	internal static EncounterBlueprintData BuildKayceeBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Zombie },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new() { },
			new() { bp_Skeleton },
			new() { bp_Revenant },
			new() { },
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Skeleton, bp_Draugr },
			new() { },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { bp_Skeleton },
			new() { },
			new() { bp_DrownedSoul }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, bp_Draugr },
			new() { bp_Draugr, bp_Family },
			new(),
			new() { bp_Skeleton },
			new() { bp_Family }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Skeleton },
			new() { bp_Zombie, bp_Bonehound },
			new(),
			new() { bp_Skeleton },
			new() { bp_Zombie }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Draugr },
			new() { bp_Project },
			new(),
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new(),
			new() { bp_Project }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new(),
			new() { bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Skeleton },
			new(),
			new() { bp_FrankAndStein },
			new(),
			new() { bp_Zombie }
		};
		return blueprint;
	}

	private static EncounterBlueprintData BuildKayceeRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Draugr },
			new(),
			new() { bp_Draugr, bp_Zombie },
			new(),
			new() { bp_Zombie },
			new(),
			new() { bp_Zombie, bp_Zombie },
			new() { bp_Draugr }
		};

		return blueprint;
	}

	internal static EncounterBlueprintData BuildRoyalBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
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
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GhostShip, bp_BonePrince },
			new(),
			new() { bp_BonePrince },
			new(),
			new() { bp_BonePrince, bp_BonePrince },
			new(),
			new(),
			new() { bp_GhostShip },
			new(),
			new() { bp_BonePrince }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_EmberSpirit },
			new(),
			new() { bp_Zombie, bp_Zombie },
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
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Dybbuk },
			new(),
			new() { bp_Revenant },
			new(),
			new() { bp_Skelemancer },
			new() { bp_Skelemancer },
			new() { bp_Dybbuk, bp_Dybbuk },
			new(),
			new() { bp_Skelemancer }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Silbon },
			new() {  },
			new() { bp_ArmoredZombie },
			new() { bp_Zombie },
			new() {  },
			new() { bp_Zombie, bp_Zombie },
			new() { bp_DrownedSoul },
			new(),
			new(),
			new() { bp_ArmoredZombie },
			new() { bp_ArmoredZombie }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildRoyalBossRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_ScreamingSkull },
			new() { bp_Obol, bp_Obol },
			new() ,
			new() { bp_ScreamingSkull },
			new() ,
			new() { bp_ScreamingSkull },
			new() { bp_Obol, bp_Obol },
			new() { bp_ScreamingSkull }
		};

		return blueprint;
	}

	internal static EncounterBlueprintData BuildGrimoraBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Silbon },
			new() { bp_Obol },
			new() { bp_Silbon },
			new() { bp_FrankAndStein },
			new() { bp_Family },
			new(),
			new() { bp_Skeleton },
			new(),
			new() { bp_Skeleton, bp_PlagueDoctor },
			new(),
			new() { bp_Revenant },
			new(),
			new() { bp_Sarcophagus },
			new() { bp_SkeletonMage }
		};
		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Project },
			new() { bp_Project },
			new() { bp_Zombie, bp_Zombie, bp_Zombie },
			new() { bp_Revenant },
			new() { bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton, bp_Skeleton, bp_Skeleton },
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Skeleton, bp_Skeleton, bp_Skeleton, bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton, bp_Skeleton, bp_Skeleton },
			new() { bp_Revenant },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Mummy, bp_TombRobber },
			new(),
			new() { bp_Mummy },
			new() { bp_Mummy, bp_TombRobber },
			new() { bp_TombRobber },
			new() { bp_Mummy },
			new() { bp_TombRobber },
			new() { bp_Mummy },
			new() { bp_DeadPets },
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			// bp_Zombie.difficultyReplace = false;
			new() { bp_ScreamingSkull },
			new() { bp_ScreamingSkull },
			new(),
			new() { bp_Zombie, bp_Zombie, bp_Zombie },
			new(),
			new() { bp_ScreamingSkull },
			new() { bp_Zombie },
			new(),
			new() { bp_ScreamingSkull },
			new() { bp_Zombie, bp_Hydra }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildGrimoraBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Revenant, bp_Skeleton },
			new() { bp_Revenant },
			new() { bp_Revenant },
			new(),
			new() { bp_Revenant, bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton },
			new(),
			new() { bp_Revenant, bp_Revenant, bp_Revenant, bp_Revenant },
		};

		return blueprint;
	}

	internal static EncounterBlueprintData BuildSawyerBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_PlagueDoctor },
			new(),
			new() {  bp_PlagueDoctor },
			new(),
			new() { bp_Skeleton, bp_PlagueDoctor },
			new(),
			new() { bp_PlagueDoctor },
			new(),
			new() { bp_DeadPets, bp_DeadPets },
			new() { bp_PlagueDoctor, bp_PlagueDoctor }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Poltergeist },
			new() { bp_Sarcophagus },
			new(),
			new(),
			new() { bp_Poltergeist },
			new(),
			new() { bp_Sarcophagus },
			new(),
			new() { bp_Poltergeist }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound },
			new() { bp_Revenant },
			new(),
			new() { bp_PlagueDoctor },
			new(),
			new(),
			new() { bp_PlagueDoctor },
			new() { bp_Bonehound }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Skelemancer },
			new(),
			new() { bp_Bonehound },
			new() { bp_Skelemancer },
			new(),
			new(),
			new() { bp_SkeletonMage, bp_Draugr },
			new() { bp_Draugr },
			new() { bp_Skelemancer },
			new(),
			new(),
			new() { bp_Bonehound, bp_Skelemancer },
			new(),
			new() { bp_SkeletonMage }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skelemancer, bp_Skelemancer },
			new() { bp_Obol },
			new(),
			new() { bp_SkeletonMage },
			new() { bp_Skelemancer },
			new(),
			new(),
			new() { bp_SkeletonMage, bp_Draugr },
			new() { bp_Obol },
			new() { bp_Skelemancer },
			new(),
			new(),
			new() { bp_Bonehound, bp_Skelemancer, bp_Skelemancer },
			new() { bp_SkeletonMage }
		};

		return blueprint;
	}

	private static EncounterBlueprintData BuildSawyerRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() ,
			new() { bp_Ripper },
			new() { bp_Ripper }
		};

		return blueprint;
	}

	#endregion
}
