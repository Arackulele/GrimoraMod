using System.Reflection;
using BepInEx;

namespace GrimoraMod;

public static class FileUtils
{
	private static readonly string[] GrimoraPluginsDir = Directory.GetFiles(
		GetDir(), "*", SearchOption.AllDirectories
	);

	private static string GetDir()
	{
		return ConfigHelper.Instance.isHotReloadEnabled 
			? Path.Combine(Paths.BepInExRootPath, "plugins/Arackulele-GrimoraMod")
			: Paths.PluginPath.Replace("GrimoraMod.dll", "");
	}

	public static byte[] ReadFileAsBytes(string file)
	{
		return File.ReadAllBytes(FindFileInPluginDir(file));
	}

	public static string FindFileInPluginDir(string file)
	{
		GrimoraPlugin.Log.LogDebug($"Looking for file [{file}]");
		return GrimoraPluginsDir.Single(str => Path.GetFileName(str) == file);
	}
}
