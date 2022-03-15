﻿using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class SeaLegs : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		return ApiUtils.CreateAbility<SeaLegs>(rulebookDescription);
	}
}
