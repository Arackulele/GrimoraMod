﻿using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePiratePrivateer = "GrimoraMod_PiratePrivateer";

	private void Add_PiratePrivateer()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Sniper)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetNames(NamePiratePrivateer, "Privateer")
			.Build();
	}
}
