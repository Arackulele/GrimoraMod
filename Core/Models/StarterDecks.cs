using BepInEx.Bootstrap;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class StarterDecks
{
	public static string DefaultStarterDeck { get; private set; }

	private static StarterDeckInfo CreateStarterDeckInfo(string title,  params string[] cards)
	{
		// Texture2D icon = TextureHelper.GetImageAsTexture($"{iconKey}.png", typeof(StarterDecks).Assembly);
		StarterDeckInfo info = ScriptableObject.CreateInstance<StarterDeckInfo>();
		info.name = $"Grimora_{title}";
		info.title = title;
		info.cards = cards.Select(i => i.GetCardInfo()).ToList();
		return info;
	}

	public static List<List<string>> decks = new List<List<string>>()
	{
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameZombie, GrimoraPlugin.NameFranknstein
		},
		new List<string>()
		{
			GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameApparition, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameDrownedSoul
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameDybbuk, GrimoraPlugin.NameDoll, GrimoraPlugin.NameDeathKnell
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameRevenant, GrimoraPlugin.NameAnimator
		},

		// new decks
		new List<string>()
		{
			GrimoraPlugin.NameDraugr, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameTombRobber, GrimoraPlugin.NameFamily, GrimoraPlugin.NameGlacier
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameObol, GrimoraPlugin.NameZombie, GrimoraPlugin.NameBonehound, GrimoraPlugin.NameBonehound
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameSummoner, GrimoraPlugin.NamePirateFirstMateSnag, GrimoraPlugin.NamePirateCaptainYellowbeard
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBoneLordsHorn, GrimoraPlugin.NameGiant, GrimoraPlugin.NameGiant
		},

	};

	public static List<List<string>> loosingdecks = new List<List<string>>()
	{
		new List<string>()
		{
			GrimoraPlugin.NameVampire, GrimoraPlugin.NameNosferat, GrimoraPlugin.NameBanshee, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameSporedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameApparition, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameDanseMacabre, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameDalgyal, GrimoraPlugin.NameEidolon, GrimoraPlugin.NameCompoundFracture, GrimoraPlugin.NameBooHag, GrimoraPlugin.NameSporedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameSarcophagus, GrimoraPlugin.NameSarcophagus, GrimoraPlugin.NameDeadPets, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameEgyptMummy, GrimoraPlugin.NameEgyptMummy, GrimoraPlugin.NameBoneless, GrimoraPlugin.NameTombRobber, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameHydra, GrimoraPlugin.NameGravebard, GrimoraPlugin.NameGravebard, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameZombie, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameTombRobber, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameDeadPets
		},
		new List<string>()
		{
			GrimoraPlugin.NameEmberSpirit, GrimoraPlugin.NameFlameskull, GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameRot, GrimoraPlugin.NameWight, GrimoraPlugin.NameFesteringWretch, GrimoraPlugin.NameSporedigger, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravebard, GrimoraPlugin.NameGravebard, GrimoraPlugin.NameObol, GrimoraPlugin.NameDisturbedGrave, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameBooHag, GrimoraPlugin.NameBooHag, GrimoraPlugin.NameBooHag, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameOurobones, GrimoraPlugin.NameBoneLordsHorn, GrimoraPlugin.NameBoneLordsHorn, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameSkeletonArmy, GrimoraPlugin.NamePirateCaptainYellowbeard, GrimoraPlugin.NameAnimator
		},
		new List<string>()
		{
			GrimoraPlugin.NameNixie, GrimoraPlugin.NameLaLlorona, GrimoraPlugin.NameGhostShip, GrimoraPlugin.NameDrownedSoul, GrimoraPlugin.NameMoroi
		},
		new List<string>()
		{
			GrimoraPlugin.NameSkelemagus, GrimoraPlugin.NameSkelemagus, GrimoraPlugin.NameSkelemagus, GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameWillOTheWisp
		},
		new List<string>()
		{
			GrimoraPlugin.NameFylgja, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameDraugr, GrimoraPlugin.NameSkeletonArmy, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameMassGrave, GrimoraPlugin.NameBoneclaw, GrimoraPlugin.NameSkeletonArmy, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameDavyJones, GrimoraPlugin.NamePirateFirstMateSnag, GrimoraPlugin.NamePiratePrivateer, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameGhostShip
		},
		new List<string>()
		{
			GrimoraPlugin.NameNecromancer, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameZombie, GrimoraPlugin.NameDeadPets, GrimoraPlugin.NameRipper
		},
		new List<string>()
		{
			GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameBloodySack
		},
		new List<string>()
		{
			GrimoraPlugin.NameAnimator, GrimoraPlugin.NamePiratePolly, GrimoraPlugin.NameRipper, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameBigbones, GrimoraPlugin.NameCenturion, GrimoraPlugin.NameGiant, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameBonepile
			//bone heap strongest man
		},
		new List<string>()
		{
			GrimoraPlugin.NameOurobones, GrimoraPlugin.NameDeadPets, GrimoraPlugin.NameRipper, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameNecromancer
		},
		new List<string>()
		{
			GrimoraPlugin.NameMummy, GrimoraPlugin.NameBoneclaw, GrimoraPlugin.NameEidolon, GrimoraPlugin.NameEgyptMummy, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameSporedigger, GrimoraPlugin.NameNecromancer, GrimoraPlugin.NameMoroi, GrimoraPlugin.NameJikininki, GrimoraPlugin.NameBalBal
		},
		new List<string>()
		{
			GrimoraPlugin.NameIceCube, GrimoraPlugin.NameEmberSpirit, GrimoraPlugin.NamePirateExploding, GrimoraPlugin.NameFlameskull, GrimoraPlugin.NameIceCube
		},
		new List<string>()
		{
			GrimoraPlugin.NameBoneless, GrimoraPlugin.NameBoneless, GrimoraPlugin.NameEidolon, GrimoraPlugin.NameEgyptMummy, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameShipwreckDams, GrimoraPlugin.NameShipwreckDams, GrimoraPlugin.NameForgottenMan, GrimoraPlugin.NameLaLlorona, GrimoraPlugin.NameSporedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameDeadHand, GrimoraPlugin.NameDeadeye, GrimoraPlugin.NamePossessedArmour, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameWillOTheWisp
		},
		new List<string>()
		{
			GrimoraPlugin.NamePossessedArmour, GrimoraPlugin.NameObol, GrimoraPlugin.NameSkelemagus, GrimoraPlugin.NameVengefulSpirit, GrimoraPlugin.NameMoroi
		},
		new List<string>()
		{
			GrimoraPlugin.NameGraveCarver, GrimoraPlugin.NameApparition, GrimoraPlugin.NameApparition, GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameSporedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameVellum, GrimoraPlugin.NameVellum, GrimoraPlugin.NameVellum, GrimoraPlugin.NameVellum, GrimoraPlugin.NameCatacomb
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameDisturbedGrave, GrimoraPlugin.NameSarcophagus, GrimoraPlugin.NameMassGrave
		},
		new List<string>()
		{
			GrimoraPlugin.NamePirateExploding, GrimoraPlugin.NamePirateExploding, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NamePirateSwashbuckler, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameMoroi, GrimoraPlugin.NameEctoplasm, GrimoraPlugin.NameTombRobber
		},
		new List<string>()
		{
			GrimoraPlugin.NameApparition, GrimoraPlugin.NameApparition, "Amoeba", "Amoeba", GrimoraPlugin.NameBloodySack
		},
		new List<string>()
		{
			GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameHaltia, GrimoraPlugin.NameWarthr, GrimoraPlugin.NameDeadHand, GrimoraPlugin.NameHellhand
		},
		new List<string>()
		{
			GrimoraPlugin.NameBoneclaw, GrimoraPlugin.NameEidolon, GrimoraPlugin.NameBoneless, GrimoraPlugin.NameMummy, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameScreamingSkull, GrimoraPlugin.NameScreamingSkull
		},
		new List<string>()
		{
			GrimoraPlugin.NameOurobones, GrimoraPlugin.NameHydra, GrimoraPlugin.NameEctoplasm, GrimoraPlugin.NameBonepile, GrimoraPlugin.NameGravedigger
		},
		new List<string>()
		{
			GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameBloodySack, GrimoraPlugin.NameApparition, GrimoraPlugin.NameApparition, GrimoraPlugin.NameDeadManWalking
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameCompoundFracture, GrimoraPlugin.NameCompoundFracture, GrimoraPlugin.NameObol, GrimoraPlugin.NameWrither
		},
		new List<string>()
		{
			GrimoraPlugin.NameGravedigger, GrimoraPlugin.NameGratefulDead, GrimoraPlugin.NameManananggal, GrimoraPlugin.NameFlameskull, GrimoraPlugin.NameSluagh
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameSporedigger, GrimoraPlugin.NamePoltergeist, GrimoraPlugin.NameHauntedMirror, GrimoraPlugin.NameHeadlessHorseman
		},
		new List<string>()
		{
			GrimoraPlugin.NameJikininki, GrimoraPlugin.NameFamily, GrimoraPlugin.NameBoneCollective, GrimoraPlugin.NameFesteringWretch, GrimoraPlugin.NameBonepile
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameSummoner, GrimoraPlugin.NameWechuge, GrimoraPlugin.NameProject, GrimoraPlugin.NameScreamingSkull
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameShipwreckDams, GrimoraPlugin.NameForgottenMan, GrimoraPlugin.NameVampire, GrimoraPlugin.NameStarvedMan
		},
		new List<string>()
		{
			GrimoraPlugin.NameBonepile, GrimoraPlugin.NameSkeleton, GrimoraPlugin.NameRevenant, GrimoraPlugin.NameSkelemagus, GrimoraPlugin.NameRipper
		},
		new List<string>()
		{
			GrimoraPlugin.NameFranknstein, GrimoraPlugin.NameFamily, GrimoraPlugin.NameDeadHand, GrimoraPlugin.NameWillOTheWisp, GrimoraPlugin.NameGravedigger
			//fuck this shit i have had to write so many of these why were there so many submissions
		},

	};



	public static List<string> deckNames = new List<string>()
	{
		"Basic",
		"Ghost",
		"Scream",
		"Skeleton",
		"Kaycee",
		"Sawyer",
		"Royal",
		"Grimora",
		"Random",
		"DavyJones",
		"Collective",
		"RunningForLife",
		"Gunners",
		"Familiar?",
		"Los Mitos",
		"Europa"
	};
	
	
	
	

	public static void RegisterStarterDecks()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameRandomCard
		});

			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameGratefulDead,
			GrimoraPlugin.NameGhostShip,
			GrimoraPlugin.NameBigbones,
			GrimoraPlugin.NameDeadManWalking,
			GrimoraPlugin.NameSilbon

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameBonepile,
			GrimoraPlugin.NameNecromancer,
			GrimoraPlugin.NameFamily,
			GrimoraPlugin.NameSkeletonArmy,
			GrimoraPlugin.NameBoneCollective

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameCrossBones,
			GrimoraPlugin.NameGhostShip,
			GrimoraPlugin.NamePiratePolly,
			GrimoraPlugin.NamePiratePrivateer,
			GrimoraPlugin.NameDavyJones

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameGratefulDead,
			GrimoraPlugin.NameWillOTheWisp,
			GrimoraPlugin.NamePiratePrivateer,
			GrimoraPlugin.NamePiratePrivateer,
			GrimoraPlugin.NameSlingersSoul

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameHydra,
			GrimoraPlugin.NameGravebard,
			GrimoraPlugin.NameCrossBones,
			GrimoraPlugin.NameVellum,
			GrimoraPlugin.NameVellum

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameBonepile,
			GrimoraPlugin.NameBonepile,
			GrimoraPlugin.NameSilbon,
			GrimoraPlugin.NameCalaveraCatrina,
			GrimoraPlugin.NameLaLlorona

		});
			decks.Add(new List<string>()
		{
			GrimoraPlugin.NameBonepile,
			GrimoraPlugin.NameGravebard,
			GrimoraPlugin.NameCenturion,
			GrimoraPlugin.NamePlagueDoctor,
			GrimoraPlugin.NamePossessedArmour

		});
		}



		for (int i = 0; i < decks.Count(); i++)
		{
	#if DEBUG
				GrimoraPlugin.Log.LogInfo(i);
	#endif

			var d = StarterDeckManager.Add(
				GrimoraPlugin.GUID,
				CreateStarterDeckInfo(deckNames[i], decks[i].ToArray())
			);

			if (i < 2)
			{
				d.Info.iconSprite = GrimoraPlugin.AllSprites.Find(o => o.name == "deck_" + deckNames[i].ToLowerInvariant());
				if (i == 0)
					DefaultStarterDeck = d.Info.name;

			}

			switch(i)
			{


				default:
				case 0:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("deck_basic");
					break;

				case 1:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("deck_ghost");
					break;

				case 2:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("PSIcon");
					break;


				case 3:
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("AMIcon");
					break;

				case 4:
				//d.UnlockLevel = 2;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("KCIcon");
					break;

				case 5:
				//d.UnlockLevel = 3;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("SYIcon");
					break;

				case 6:
				//d.UnlockLevel = 4;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("RYIcon");
					break;

				case 7:
				//d.UnlockLevel = 5;
				d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("GMIcon");
					break;

				case 8:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("RNIcon");
					break;

				case 9:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("RLIcon");
					break;

				case 10:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("BCIcon");
					break;

				case 11:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("DJIcon");
					break;

				case 12:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("SNIcon");
					break;

				case 13:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("HDIcon");
					break;

				case 14:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("LMIcon");
					break;

				case 15:
					//d.UnlockLevel = 5;
					d.Info.iconSprite = AssetUtils.GetPrefab<Sprite>("EUIcon");
					break;
			}
	}
		
		StarterDeckManager.ModifyDeckList += delegate(List<StarterDeckManager.FullStarterDeck> decks)
		{
			CardTemple acceptableTemple = ScreenManagement.ScreenState;

			// Only keep decks where at least one card belongs to this temple
			decks.RemoveAll(info => info.Info.cards.FirstOrDefault(ci => ci.temple == acceptableTemple) == null);


			return decks;
		};
	}
}
