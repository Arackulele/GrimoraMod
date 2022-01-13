using System.Collections;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public class FlameStrafe : Strafe
	{
		public static Ability ability;

		public override Ability Ability => ability;

		// Token: 0x06001419 RID: 5145 RVA: 0x000444BC File Offset: 0x000426BC
		public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
		{
			if (cardSlot.Card == null)
			{
				yield return BoardManager
					.Instance
					.CreateCardInSlot(CardLoader.GetCardByName(GrimoraPlugin.NameFlames), cardSlot);
			}

			yield break;
		}

		public static NewAbility CreateFlameStrafe()
		{
			const string rulebookDescription =
				"Whenever [creature] moves, it leaves a trail of Embers. " +
				"The warmth of the Embers shall enlighten nearby cards.";

			return ApiUtils.CreateAbility<FlameStrafe>(
				Resources.DropFlames,
				nameof(FlameStrafe),
				rulebookDescription,
				5
			);
		}
	}
}