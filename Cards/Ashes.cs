using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameAshes = $"{GUID}_Ashes";

	private void Add_Card_Ashes()
	{
		CardBuilder.Builder
			.SetBoneCost(1)
			.SetBaseAttackAndHealth(0, 0)
			.SetDescription("Only a fool would need such assistance.")
			.SetNames(NameAshes, "Graven Ashes")
			.Build();
	}
}
