using System.Reflection;
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
		return ConfigHelper.Instance.isHotReloadEnabled
			? Path.Combine(Paths.BepInExRootPath, "plugins", "Arackulele-GrimoraMod")
			: Assembly.GetExecutingAssembly().Location.Replace("GrimoraMod.dll", "");
	}

	public static byte[] ReadFileAsBytes(string file, bool isPng = false)
	{
		return File.ReadAllBytes(FindFileInPluginDir(file, isPng));
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
}
