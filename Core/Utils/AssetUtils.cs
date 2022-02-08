using UnityEngine;

namespace GrimoraMod;

public static class AssetUtils
{
	public static T[] LoadAssetBundle<T>(string assetBundle) where T : UnityEngine.Object
	{
		AssetBundle abilityBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir(assetBundle));
		var loadedBundle = abilityBundle.LoadAllAssets<T>();
		// GrimoraPlugin.Log.LogDebug($"{string.Join(",", loadedBundle.Select(_ => _.name))}");
		abilityBundle.Unload(false);
		return loadedBundle;
	}

}
