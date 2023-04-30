using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiantEphialtes = $"{GUID}_GiantEphialtes";

	private void Add_Card_TwinGiant_Ephialtes()
	{
		Sprite giantSprite = AllSprites.Single(spr => spr.name == "Giant");
		CardBuilder.Builder
		 .SetAbilities(Ability.MadeOfStone, Ability.Reach, GiantStrike.ability)
		 .SetSpecialAbilities(GrimoraGiant.FullSpecial.Id)
		 .SetBaseAttackAndHealth(1, 8)
		 .SetBoneCost(15)
		 .SetNames(NameGiantEphialtes, "Ephialtes", giantSprite)
		 .SetTraits(Trait.Giant, Trait.Uncuttable)
		 .Build()
			;
	}
}
