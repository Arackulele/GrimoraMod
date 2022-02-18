using DiskCardGame;

namespace GrimoraMod;

public static class StringExtensions
{
	public static CardInfo GetCardInfo(this string self)
	{
		return CardLoader.GetCardByName(self);
	}

	public static string GetDelimitedString<T>(this IEnumerable<T> self) where T : UnityEngine.Object
	{
		return string.Join(",", self.Select(_ => _.name));
	}
	
	public static string GetDelimitedString(this string[] self)
	{
		return string.Join(",", self);
	}
	
	public static string GetDelimitedString(this IEnumerable<Object> self)
	{
		return string.Join(",", self.Select(_ => _.name));
	}
}
