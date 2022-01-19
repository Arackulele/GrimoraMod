using BepInEx;

namespace GrimoraMod;

public static class FileUtils
{
	public static byte[] ReadFileAsBytes(string file)
	{
		return File.ReadAllBytes(FindFileInPluginDir(file));
	}

	public static string FindFileInPluginDir(string file)
	{
		return Directory.GetFiles(Paths.PluginPath, file, SearchOption.AllDirectories)[0];
	}
}