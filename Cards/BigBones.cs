using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBigbones = $"{GUID}_Bigbones";

	private void Add_Card_Bigbones()
	{
		string DisplayName = "Big Bones";
		List<String> FunnyNames = new List<String> { "Burly Bones", "Beefy Bones", "Cal C. Um", "Sizeable Bones", "Considerable Bones", "Vigorous Bones", "Vigorous Bones", "Hefty Bones" };
		if (UnityEngine.Random.Range(1, 100) < 6) DisplayName = FunnyNames.GetRandomItem();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(2, 3)
			.SetAbilities(Ability.StrafePush)
			.SetBoneCost(6)
			.SetDescription("A RATHER BURLY SKELETON, IT IS VERY RICH IN CALCIUM.")
			.SetNames(NameBigbones, DisplayName)
			.Build();
	}
}
