using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod
{
	public class KayceeBossExt : BaseBossExt
	{
		// public const Type Opponent = (Type)1001;

		public override StoryEvent EventForDefeat => StoryEvent.FactoryConveyorBeltMoved;

		public override Type Opponent => KayceeOpponent;

		public override string DefeatedPlayerDialogue => "Youuuuuuu, painnnfulllll deaaathhh awaiiitttsss youuuuuuu!";

		public override EncounterBlueprintData BuildInitialBlueprint()
		{
			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() { bp_Skeleton },
				new() { bp_DrownedSoul },
				new() { bp_Draugr },
				new() { bp_Skeleton },
				new() { },
				new() { },
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

		public EncounterBlueprintData BuildNewPhaseBlueprint()
		{
			var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
			blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>
			{
				new() { bp_UndeadWolf },
				new() { },
				new() { bp_Draugr },
				new() { },
				new() { bp_Zombie },
				new() { },
				new() { bp_Zombie },
				new() { },
				new() { },
				new() { bp_Draugr },
				new() { bp_Zombie },
				new() { },
				new() { },
				new() { bp_HeadlessHorseman },
				new() { bp_Draugr }
			};

			return blueprint;
		}

		public override IEnumerator StartNewPhaseSequence()
		{
			{
				yield return this.ClearBoard();
				var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
				foreach (var playerSlot in playerSlotsWithCards)
				{
					// card.SetIsOpponentCard();
					// card.transform.eulerAngles += new Vector3(0f, 0f, -180f);
					yield return BoardManager.Instance.CreateCardInSlot(
						playerSlot.Card.Info, playerSlot.opposingSlot, 0.25f
					);
				}

				yield return base.ReplaceBlueprintCustom(BuildNewPhaseBlueprint());
			}
			yield break;
		}
	}
}