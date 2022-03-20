using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneCollective = $"{GUID}_BoneCollective";

	private void Add_BoneCollective()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.ActivatedStatsUp, Ability.Submerge)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(2)
			.SetNames(NameBoneCollective, "Bone Collective")
			// .SetDescription("NOT YOUR ORDINARY UNDEAD, THEY SEARCHED THROUGH A SCRAPYARD FOR THIS GEAR.")
			.Build();
	}
}
