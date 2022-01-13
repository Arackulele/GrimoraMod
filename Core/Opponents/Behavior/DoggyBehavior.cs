using System.Collections;
using DiskCardGame;

namespace GrimoraMod
{
	public class DoggyBehavior : BossBehaviour
	{
		public IEnumerator OnOtherCardDie(CardSlot otherCard)
		{
			yield return BoardManager.Instance.CreateCardInSlot(
				CardLoader.GetCardByName("Bonehound"), otherCard);
			yield break;
		}
	}
}