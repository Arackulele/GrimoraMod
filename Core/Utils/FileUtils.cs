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

	public static void CheckIfDirectoriesNeededExist()
	{
		const string exceptionToPrint = "Please make sure if you manually installed that you copied everything in the zip file!";
		
		if (!Directory.Exists(Path.Combine(GetDir(), "DataFiles")))
		{
			throw new DirectoryNotFoundException($"Unable to find DataFiles directory in [{GrimoraPluginsDir}]! {exceptionToPrint}");
		}

		if (!Directory.Exists(Path.Combine(GetDir(), "Artwork")))
		{
			throw new DirectoryNotFoundException($"Unable to find Artwork directory in [{GrimoraPluginsDir}]! {exceptionToPrint}");
		}
		
		Log.LogDebug($"[CheckIfDirectoriesNeededExist] Both directories exist! Continuing loading rest of mod.");
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
		catch (InvalidOperationException e)
		{
			Log.LogError($"Unable to find file [{Path.GetFileName(file)}] in directory [{GrimoraPluginsDir}] ! " +
			             $"Are you sure you have both Artwork and DataFiles in this directory?");
			throw;
		}
	}
}
