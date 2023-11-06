using System.Collections;
using DiskCardGame;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class FlameStrafe : Strafe
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
	{
		if (oldSlot.Card.SafeIsUnityNull())
		{
			yield return oldSlot.CreateCardInSlot(NameFlames.GetCardInfo());
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_FlameStrafe()
	{
		//For some reason i cant use NameFlames???
		const string rulebookDescription =
			$"Whenever [creature] moves, it leaves a trail of Embers. [define:{"arackulele.inscryption.grimoramod_Flames"}]";

		AbilityBuilder<FlameStrafe>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
