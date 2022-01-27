using System.Reflection;

namespace GrimoraMod;

public static class FileUtils
{
	private static readonly string[] GrimoraPluginsDir = Directory.GetFiles(
		Assembly.GetExecutingAssembly().Location.Replace("GrimoraMod.dll", ""),
		"*",
		SearchOption.AllDirectories
	);

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
