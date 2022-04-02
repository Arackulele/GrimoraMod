using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneCollective = $"{GUID}_BoneCollective";

	private void Add_Card_BoneCollective()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.ActivatedStatsUp, Ability.Submerge)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(2)
			.SetNames(NameBoneCollective, "Bone Collective")
			.SetDescription("ITS THOUSANDS OF TINY BONES COALESCE INTO A HUMANOID FORM ONLY TO DISPERSE IN A CLATTERING SWARM THE NEXT MOMENT.")
			.Build();
	}
}
