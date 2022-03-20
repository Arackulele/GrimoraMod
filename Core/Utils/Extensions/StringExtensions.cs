using DiskCardGame;
using Sirenix.Utilities;

namespace GrimoraMod;

public static class StringExtensions
{

	public static bool IsNotEmpty(this string self)
	{
		return !self.IsNullOrWhitespace();
	}
	public static string GetDelimitedString<T>(this IEnumerable<T> self, string delimiter = ",") where T : UnityEngine.Object
	{
		return string.Join(delimiter, self.Select(_ => _.name));
	}
	
	public static string GetDelimitedString<T>(this List<T> self, string delimiter = ",")
	{
		return string.Join(delimiter, self);
	}

	#region ColorCodes

	public static string DarkBlue(this string self)
	{
		return "[c:dB]" + self + "[c:]";
	}
	
	public static string Gold(this string self)
	{
		return "[c:G]" + self + "[c:]";
	}
	
	public static string BrightGold(this string self)
	{
		return "[c:bG]" + self + "[c:]";
	}
	
	public static string BrightBlue(this string self)
	{
		return "[c:bB]" + self + "[c:]";
	}
	
	public static string Blue(this string self)
	{
		return "[c:B]" + self + "[c:]";
	}
	
	public static string BrownOrange(this string self)
	{
		return "[c:brnO]" + self + "[c:]";
	}
	
	public static string Orange(this string self)
	{
		return "[c:O]" + self + "[c:]";
	}
	
	public static string Red(this string self)
	{
		return "[c:R]" + self + "[c:]";
	}
	
	public static string BrightRed(this string self)
	{
		return "[c:bR]" + self + "[c:]";
	}
	
	public static string DarkLimeGreen(this string self)
	{
		return "[c:dlGr]" + self + "[c:]";
	}
	
	public static string LimeGreen(this string self)
	{
		return "[c:lGr]" + self + "[c:]";
	}

	public static string BrightLimeGreen(this string self)
	{
		return "[c:blGr]" + self + "[c:]";
	}
	
	public static string DarkSeafoam(this string self)
	{
		return "[c:dSG]" + self + "[c:]";
	}
	
	public static string GlowSeafoam(this string self)
	{
		return "[c:bSG]" + self + "[c:]";
	}
	
	#endregion
}
