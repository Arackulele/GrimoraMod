using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public class Anchored : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Anchored()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		AbilityBuilder<Anchored>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("anchor_pixel"))
		 .Build();
	}
}
