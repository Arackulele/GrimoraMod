using UnityEngine;

namespace GrimoraMod;

public static class AssetUtils
{
	public static T[] LoadAssetBundle<T>(string assetBundleFile) where T : UnityEngine.Object
	{
		AssetBundle assetBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir(assetBundleFile));
		var loadedBundle = assetBundle.LoadAllAssets<T>();
		// GrimoraPlugin.Log.LogDebug($"Bundle [{assetBundle}] - {string.Join(",", loadedBundle.Select(_ => _.name))}");
		assetBundle.Unload(false);
		return loadedBundle;
	}

}
