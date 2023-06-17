using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadManWalking = $"{GUID}_DeadManWalking";



	private void Add_Card_DeadManWalking()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Collector.ability, Ability.Strafe)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(6)
			.SetDescription("A DOOMED FATE SHOULD DISCOURAGE ONE FROM GOING ON, AT LEAST THATS WHAT A STILL LIVING MAN WOULD TELL YOU.")
			.SetNames(NameDeadManWalking, "Dead Man Walking")
			.Build();
	}
}
