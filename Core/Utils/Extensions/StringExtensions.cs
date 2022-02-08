using DiskCardGame;

namespace GrimoraMod;

public static class StringExtensions
{
	public static CardInfo GetCardInfo(this string self)
	{
		return CardLoader.GetCardByName(self);
	}
}
