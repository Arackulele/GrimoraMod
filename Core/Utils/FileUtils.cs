using System.IO.Compression;
using System.Reflection;
using System.Text;
using BepInEx;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class FileUtils
{
	private static readonly string GrimoraPluginsDir = GetDir();
	
	private static readonly string[] FilesToSearch = Directory.GetFiles(
		GrimoraPluginsDir, "*", SearchOption.AllDirectories
	);

	private static string GetDir()
	{
		return ConfigHelper.Instance.IsHotReloadEnabled
			? Path.Combine(Paths.BepInExRootPath, "plugins", "Arackulele-GrimoraMod")
			: Assembly.GetExecutingAssembly().Location.Replace("GrimoraMod.dll", string.Empty);
	}

	public static string FindFileInPluginDir(string file, bool isPng = false)
	{
		if (isPng && !file.EndsWith(".png"))
		{
			file += ".png";
		}
		
		Log.LogDebug($"Looking for file [{file}]");
		try
		{
			return FilesToSearch.Single(str => Path.GetFileName(str) == file);
		}
		catch (Exception e)
		{
			Log.LogError($"Unable to find file [{Path.GetFileName(file)}] in directory [{GrimoraPluginsDir}] ! " +
			             $"These are the files I have found: [{string.Join(",", FilesToSearch.Select(Path.GetFileName))}]");
			throw;
		}
	}
	
	public static string ToCompressedJSON(object data)
	{
		if (data == null)
			return default;
		
		string value = SaveManager.ToJSON(data);
		var bytes = Encoding.Unicode.GetBytes(value);
		using MemoryStream input = new MemoryStream(bytes);
		using MemoryStream output = new MemoryStream();
		using (GZipStream stream = new GZipStream(output, CompressionLevel.Optimal))
		{
			input.CopyTo(stream);
		}
		return Convert.ToBase64String(output.ToArray());
	}

	public static T FromCompressedJSON<T>(string data)
	{
		if (string.IsNullOrEmpty(data))
			return default;

		var bytes = Convert.FromBase64String(data);
		using MemoryStream input = new MemoryStream(bytes);
		using MemoryStream output = new MemoryStream();
		using(GZipStream stream = new GZipStream(input, CompressionMode.Decompress))
		{
			stream.CopyTo(output);
		}
		string json = Encoding.Unicode.GetString(output.ToArray());
		return SaveManager.FromJSON<T>(json);
	}
}
