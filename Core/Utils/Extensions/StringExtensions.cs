using DiskCardGame;

namespace GrimoraMod;

public static class StringExtensions
{
	public static CardInfo GetCardInfo(this string self)
	{
		return CardLoader.GetCardByName(self);
	}

	public static string GetDelimitedString(this string[] self)
	{
		return string.Join(",", self);
	}
	
	public static string GetDelimitedString(this UnityEngine.Object[] self)
	{
		return string.Join(",", self.Select(_ => _.name));
	}
}
