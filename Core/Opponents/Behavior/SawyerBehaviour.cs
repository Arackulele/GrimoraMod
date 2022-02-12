using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public class SawyerBehaviour : BossBehaviour
{
	public IEnumerator OnOtherCardDie(CardSlot otherCard)
	{
		yield return BoardManager.Instance.CreateCardInSlot("Bonehound".GetCardInfo(), otherCard);
	}
}
