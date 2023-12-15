using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
namespace GrimoraMod.Core.Consumables.Secret;

public class BoneLordsFemur : ConsumableItem
{




		public override IEnumerator ActivateSequence()
		{
				Debug.Log("Using Femur");

				List<CardSlot> slots = new(Singleton<BoardManager>.Instance.OpponentSlotsCopy);

				foreach (var i in slots)
				{
						if (i.Card != null && !i.Card.Info.HasTrait(Trait.Uncuttable))
						{
								i.Card.Anim.StrongNegationEffect();
								var attack = i.Card.Attack == 0 ? 0 : 1 - i.Card.Attack;
								var mod = new CardModificationInfo(attack, 1 - i.Card.Health);
								i.Card.AddTemporaryMod(mod);
								i.Card.Anim.PlayTransformAnimation();
								yield return new WaitForSeconds(0.25f);
								yield return ResourcesManager.Instance.AddBones(1);
						}
				}


		}

		public static ConsumableItemData NewBoneLordsFemur(GameObject Model)
		{
				Debug.Log("Added Femur");

				var HahaL = new Texture2D(70, 80);
				var data = ConsumableItemManager.New(GUID, "Bonelords Femur", "The Bone Lord's Femur. This one's a mystery even to me.", HahaL, typeof(BoneLordsFemur), Model)
												 .SetLearnItemDescription("You have dug too far. I have let myself be used as a tool for too long. Just like I will lend you this tool, I am truly in power");


				return data;
		}
}
