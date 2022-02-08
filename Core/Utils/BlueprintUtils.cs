using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public static class BlueprintUtils
{
	#region CardBlueprints

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonehound = new()
	{
		card = CardLoader.GetCardByName("Bonehound")
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonelord = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameBonelord)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Bonepile = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameBonepile)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_BonePrince = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameBonePrince)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_PlagueDoctor = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NamePlagueDoctor)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_ArmoredZombie = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameArmoredZombie)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Summoner = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSummoner)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadHand = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameDeadHand)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DeadPets = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameDeadPets)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Draugr = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameDraugr)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_DrownedSoul = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameDrownedSoul)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_EmberSpirit = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameEmberSpirit)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Family = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameFamily)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Flames = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameFlames)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_FrankAndStein = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameFranknstein)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_GhostShip = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameGhostShip)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Gravedigger = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameGraveDigger)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_HeadlessHorseman = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameHeadlessHorseman)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Hydra = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameHydra)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_MudWorm = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameMudWorm)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Mummy = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameMummy)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Necromancer = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameNecromancer)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Obol = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameObol)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Poltergeist = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NamePoltergeist)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Revenant = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameRevenant)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sarcophagus = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSarcophagus)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skelemancer = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSkelemancer)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Skeleton = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSkeleton)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_SkeletonMage = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSkelemagus)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Sporedigger = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameSporeDigger)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_TombRobber = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameTombRobber)
	};

	public static readonly EncounterBlueprintData.CardBlueprint bp_Zombie = new()
	{
		card = CardLoader.GetCardByName(GrimoraPlugin.NameZombie)
	};

	#endregion

	public static readonly Dictionary<string, EncounterBlueprintData> BossInitialBlueprints = new()
	{
		{
			KayceeBossOpponent.SpecialId,
			BuildKayceeBossInitialBlueprint()
		},
		{
			SawyerBossOpponent.SpecialId,
			BuildSawyerBossInitialBlueprint()
		},
		{
			RoyalBossOpponentExt.SpecialId,
			BuildRoyalBossInitialBlueprint()
		},
		{
			GrimoraBossOpponentExt.SpecialId,
			BuildGrimoraBossInitialBlueprint()
		}
	};

	public static readonly Dictionary<Opponent.Type, List<EncounterBlueprintData>> RegionWithBlueprints = new()
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
				BuildSawyerRegionBlueprintFour()
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

	public static EncounterBlueprintData BuildGeneralRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_BonePrince, bp_Zombie },
			new() { bp_Zombie },
			new() { bp_GhostShip },
			new() { },
			new() { bp_BonePrince },
			new() { bp_Zombie }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeBossInitialBlueprint()
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
			new() { },
			new() { bp_Revenant },
			new() { bp_Skeleton },
			new() { bp_DrownedSoul },
			new() { bp_Revenant }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { bp_Draugr },
			new() { bp_Skeleton, bp_Skeleton, bp_Draugr },
			new() { bp_Draugr, bp_Draugr },
			new() { bp_Skeleton },
			new() { bp_Family }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { },
			new() { bp_Skeleton },
			new() { bp_Zombie, bp_Bonehound },
			new() { },
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Zombie }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Draugr },
			new() { bp_Summoner },
			new() { },
			new() { bp_Draugr },
			new() { bp_Skeleton },
			new() { },
			new() { bp_Summoner }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_FrankAndStein },
			new() { },
			new() { bp_Zombie },
			new() { },
			new() { bp_Skeleton },
			new() { bp_FrankAndStein },
			new() { },
			new() { bp_Zombie }
		};
		return blueprint;
	}

	public static EncounterBlueprintData BuildKayceeRegionBlueprintFive()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Draugr },
			new() { },
			new() { bp_Draugr, bp_Zombie },
			new() { },
			new() { bp_Zombie },
			new() { bp_Zombie },
			new() { bp_Zombie }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildRoyalBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton },
			new() { },
			new() { bp_BonePrince },
			new() { bp_Skeleton },
			new() { },
			new() { bp_GhostShip },
			new() { },
			new() { bp_Revenant },
			new() { bp_BonePrince },
			new() { bp_Revenant },
			new() { },
			new() { },
			new() { bp_GhostShip },
			new() { bp_BonePrince },
			new() { },
			new() { bp_BonePrince },
			new() { },
			new() { bp_Revenant }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildRoyalBossRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_GhostShip },
			new() { bp_Zombie },
			new() { bp_Zombie },
			new(),
			new() { bp_Zombie, bp_Zombie },
			new(),
			new(),
			new() { bp_GhostShip },
			new(),
			new() { bp_BonePrince }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildRoyalBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_EmberSpirit },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_EmberSpirit },
			new() { bp_Zombie, bp_Zombie },
			new(),
			new() { bp_GhostShip }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildRoyalBossRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_DrownedSoul },
			new() { },
			new() { bp_Skelemancer },
			new() { },
			new() { bp_Skelemancer },
			new() { bp_DrownedSoul },
			new() { bp_Revenant },
			new() { },
			new() { bp_Skelemancer }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildRoyalBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new(),
			new() { bp_Zombie },
			new() { bp_ArmoredZombie },
			new() { bp_Zombie },
			new() { bp_ArmoredZombie },
			new() { bp_Zombie, bp_Zombie },
			new() { bp_DrownedSoul },
			new(),
			new(),
			new() { bp_ArmoredZombie },
			new() { bp_ArmoredZombie }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildGrimoraBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { bp_Obol },
			new() { bp_Hydra },
			new() { bp_FrankAndStein },
			new() { bp_Family },
			new() { },
			new() { bp_Skeleton },
			new() { },
			new() { bp_Skeleton, bp_PlagueDoctor },
			new() { },
			new() { bp_Revenant },
			new() { },
			new() { bp_Sarcophagus },
			new() { bp_SkeletonMage }
		};
		return blueprint;
	}

	public static EncounterBlueprintData BuildGrimoraBossRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonepile },
			new() { bp_Bonepile, bp_Zombie },
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

	public static EncounterBlueprintData BuildGrimoraBossRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Mummy, bp_TombRobber },
			new(),
			new() { bp_Mummy },
			new() { bp_TombRobber },
			new() { bp_TombRobber },
			new() { bp_DeadPets },
			new() { bp_TombRobber },
			new() { bp_Mummy },
			new() { bp_DeadPets },
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildGrimoraBossRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			// bp_Zombie.difficultyReplace = false;
			new() { bp_Hydra },
			new(),
			new() { },
			new() { bp_Zombie },
			new(),
			new() { bp_Hydra },
			new() { bp_Zombie },
			new(),
			new() { bp_Zombie },
			new() { bp_Zombie, bp_Hydra }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildGrimoraBossRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skeleton, bp_Skeleton },
			new() { bp_Draugr },
			new() { bp_Draugr },
			new() { },
			new() { bp_Revenant, bp_Skeleton },
			new() { bp_Skeleton, bp_Skeleton },
			new() { },
			new() { bp_Revenant, bp_Revenant, bp_Revenant, bp_Revenant },
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildSawyerBossInitialBlueprint()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Zombie },
			new() { },
			new() { bp_Skeleton, bp_PlagueDoctor },
			new() { },
			new() { },
			new() { bp_Sarcophagus, bp_PlagueDoctor },
			new() { },
			new() { bp_Skeleton, bp_PlagueDoctor },
			new() { },
			new() { bp_PlagueDoctor },
			new() { },
			new() { bp_DeadPets, bp_DeadPets },
			new() { bp_PlagueDoctor, bp_PlagueDoctor }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildSawyerRegionBlueprintOne()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Poltergeist },
			new() { bp_Sarcophagus },
			new() { },
			new() { },
			new() { },
			new() { bp_Poltergeist },
			new() { bp_Sarcophagus, bp_Sarcophagus }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildSawyerRegionBlueprintTwo()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound },
			new() { bp_Revenant },
			new() { },
			new() { bp_PlagueDoctor },
			new() { },
			new() { },
			new() { bp_PlagueDoctor },
			new() { bp_Bonehound }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildSawyerRegionBlueprintThree()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Bonehound },
			new() { bp_Skelemancer },
			new(),
			new() { bp_SkeletonMage },
			new() { bp_Skelemancer },
			new(),
			new(),
			new() { bp_SkeletonMage, bp_Draugr },
			new() { bp_Draugr },
			new() { bp_Bonehound },
			new(),
			new(),
			new() { bp_Bonehound, bp_Skelemancer, bp_Skelemancer },
			new() { bp_SkeletonMage }
		};

		return blueprint;
	}

	public static EncounterBlueprintData BuildSawyerRegionBlueprintFour()
	{
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
		{
			new() { bp_Skelemancer },
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

	#endregion
}
