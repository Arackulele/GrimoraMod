using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using System.Collections;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class DropCandy : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

		public override bool RespondsToDrawn() => true;


	public override bool RespondsToResolveOnBoard()
		{
		if (Card.slot.IsOpponentSlot()) return true;
		else return false;
		}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		GrimoraModSawyerBossSequencer.CandyCounter--;

		yield break;
	}

		public override IEnumerator OnDrawn()
		{

		int portrait = UnityEngine.Random.Range(1, 100);
		Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket");
		Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket_emission"));
		if (portrait > 33)
		{
			Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket1");
			Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket1_emission"));
		}
		if (portrait > 66)
		{
			Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket2");
			Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket2_emission"));
		}

		CardModificationInfo cardModificationInfo;


		cardModificationInfo = new CardModificationInfo
		{
			abilities = new List<Ability> { Ability.DrawRandomCardOnDeath, Ability.QuadrupleBones },
			negateAbilities = new List<Ability>() { DropCandy.ability }

		};

		Card.AddTemporaryMod(cardModificationInfo);

		yield break;
		}

		public override IEnumerator OnResolveOnBoard()
		{
		int portrait = UnityEngine.Random.Range(1, 100);
		Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket");
		Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket_emission"));
		if (portrait > 33)
		{
			Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket1");
			Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket1_emission"));
		}
		if (portrait > 66)
		{
			Card.Info.portraitTex = AssetUtils.GetPrefab<Sprite>("CandyBucket2");
			Card.Info.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("CandyBucket2_emission"));
		}

		yield break;
		}

}

public partial class GrimoraPlugin
{
	
		public void Add_Ability_DropCandy()
			{
				const string rulebookDescription = "[creature] will yield candy upon death, maybe this will be useful for later.";

				AbilityBuilder<DropCandy>.Builder
				 .SetRulebookDescription(rulebookDescription)
				 .SetRulebookName("Full o' candy")
				 .Build();
			}
}
