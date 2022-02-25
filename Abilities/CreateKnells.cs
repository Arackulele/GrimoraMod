﻿using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class CreateKnells : CreateBells
{
	public static readonly NewAbility NewAbility = Create();
	
	public static Ability ability;

	public override Ability Ability => ability;

	public override string SpawnedCardId => GrimoraPlugin.NameDeathKnellBell;

	public static NewAbility Create()
	{
		const string rulebookDescription = "When [creature] is played, a Chime is created on each empty adjacent space." +
		                                   $" [define:{GrimoraPlugin.NameDeathKnellBell}]";

		return ApiUtils.CreateAbility<CreateKnells>
			(rulebookDescription, rulebookIcon: AbilitiesUtil.LoadAbilityIcon(Ability.CreateBells.ToString()));
	}
}