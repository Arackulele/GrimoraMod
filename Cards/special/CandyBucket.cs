using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCandyBucket = $"{GUID}_CandyBucket";

	private void Add_Card_CandyBucket()
	{
		CardBuilder.Builder
		 .SetAbilities(DropCandy.ability)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetBoneCost(1)
		 .SetDescription("Trick or treat!")
		 .SetNames(NameCandyBucket, "Candy Bucket")
		 .Build();
	}
}
