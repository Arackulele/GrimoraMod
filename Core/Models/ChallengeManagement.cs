using DiskCardGame;
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Ascension;
using InscryptionAPI.Guid;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class ChallengeManagement
{
	public static AscensionChallenge NoBones { get; private set; }
	public static AscensionChallenge KayceesKerfuffle { get; private set; }
	public static AscensionChallenge SawyersShowdown { get; private set; }
	public static AscensionChallenge RoyalsRevenge { get; private set; }
	
	public static Dictionary<AscensionChallenge, AscensionChallengeInfo> PatchedChallengesReference;
	
	public static List<AscensionChallenge> ValidChallenges;
	
	public static void UpdateGrimoraChallenges()
	{
		NoBones = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "NoBones");
		KayceesKerfuffle = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "KayceesKerfuffle");
		SawyersShowdown = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SawyersShowdown");
		RoyalsRevenge = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "RoyalsRevenge");

		PatchedChallengesReference = new Dictionary<AscensionChallenge, AscensionChallengeInfo>
		{
			{
				AscensionChallenge.NoHook, 
				new()
				{
					challengeType = NoBones,
					title = "No Bones",
					description = "You no longer gain the extra bones after defeating bosses.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("NoBones"),
					pointValue = 5
				}
			},
			{
				AscensionChallenge.LessConsumables, 
				new()
				{
					challengeType = KayceesKerfuffle,
					title = "Kaycee's Kerfuffle",
					description = "The fourth turn of every battle, all your cards will are Frozen Away.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("KayceesKerfuffle"),
					pointValue = 15
				}
			},
			
			{
				AscensionChallenge.ExpensivePelts, 
				new()
				{
					challengeType = SawyersShowdown,
					title = "Sawyer's Showdown",
					description = "Lose 1 bone every 3 turns.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("SawyersShowdown"),
					pointValue = 10
				}
			},
			
			{
				AscensionChallenge.NoClover, 
				new()
				{
					challengeType = RoyalsRevenge,
					title = "Royal's Revenge",
					description = "Every third card you play gains the Lit Fuse sigil.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("RoyalsRevenge"),
					pointValue = 20
				}
			},
		};

		ValidChallenges = new List<AscensionChallenge>
		{
			AscensionChallenge.BaseDifficulty,
			AscensionChallenge.ExpensivePelts,
			AscensionChallenge.LessConsumables,
			AscensionChallenge.LessLives,
			AscensionChallenge.NoBossRares,
			AscensionChallenge.NoHook, 
			AscensionChallenge.StartingDamage,
			AscensionChallenge.WeakStarterDeck,
			AscensionChallenge.SubmergeSquirrels,
			NoBones,
			AscensionChallenge.BossTotems,
			KayceesKerfuffle,
			AscensionChallenge.AllTotems,
			SawyersShowdown,
			AscensionChallenge.NoClover,
			RoyalsRevenge,
		};





		ChallengeManager.ModifyChallenges += delegate(List<AscensionChallengeInfo> challenges)
		{
			if (ScreenManagement.ScreenState == CardTemple.Undead)
			{
				for (int i = 0; i < challenges.Count; i++)
				{
					if (PatchedChallengesReference.ContainsKey(challenges[i].challengeType))
					{
						challenges[i] = PatchedChallengesReference[challenges[i].challengeType];
					}
				}

				return challenges;
			}
			challenges =  ChallengeManager.BaseGameChallenges.ToList();
			return challenges;
		};
	}
	
	
	[HarmonyPostfix, HarmonyPatch(typeof(AscensionUnlockSchedule), nameof(AscensionUnlockSchedule.ChallengeIsUnlockedForLevel))]
	[HarmonyAfter(InscryptionAPIPlugin.ModGUID)]
	public static void ValidGrimoraChallenges(ref bool __result, AscensionChallenge challenge, int level)
	{
		if (ScreenManagement.ScreenState == CardTemple.Undead && SaveDataRelatedPatches.IsGrimoraRun) 
		{
			if (!ValidChallenges.Contains(challenge))
			{
				__result = false;
				return;
			}
		}

		if (PatchedChallengesReference.Any(kvp => kvp.Value.challengeType == challenge))
		{
			var kvp = PatchedChallengesReference.First(kvp => kvp.Value.challengeType == challenge);
			if (kvp.Value.challengeType != kvp.Key)
			{
				__result = AscensionUnlockSchedule.ChallengeIsUnlockedForLevel(kvp.Key, level);
			}
		}
	}
}
