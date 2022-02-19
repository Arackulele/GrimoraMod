using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class AssetUtils
{
	public static List<T> LoadAssetBundle<T>(string assetBundleFile) where T : UnityEngine.Object
	{
		AssetBundle assetBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir(assetBundleFile));
		var loadedBundle = assetBundle.LoadAllAssets<T>();
		// GrimoraPlugin.Log.LogDebug($"Bundle [{assetBundle}] - {string.Join(",", loadedBundle.Select(_ => _.name))}");
		assetBundle.Unload(false);
		return loadedBundle.ToList();
	}

	private static bool NameMatchesAsset(UnityEngine.Object obj, string nameToCheckFor)
	{
		return obj.name.Equals(nameToCheckFor, StringComparison.OrdinalIgnoreCase);
	}

	public static T GetPrefab<T>(string prefabName) where T : UnityEngine.Object
	{
		Type type = typeof(T);

		T objToReturn = null;
		try
		{
			if (type == typeof(Material))
			{
				objToReturn = AllMats.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
			else if (type == typeof(GameObject))
			{
				objToReturn = AllPrefabs.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
			else if (type == typeof(RuntimeAnimatorController))
			{
				objToReturn = AllControllers.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
			else if (type == typeof(Sprite))
			{
				objToReturn = AllSprites.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
			else if (type == typeof(Texture))
			{
				objToReturn = AllAbilityTextures.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
		}
		catch (Exception e)
		{
			Log.LogError($"Unable to find prefab [{prefabName}]! This could mean the asset bundle doesn't contain it, or, most likely, your mod manager didn't correctly update the asset bundle files." +
			             $"If it worked last update, delete your files and download the mod files again. " +
			             $"There's a weird issue with how mod managers handle asset bundle between mod updates.");
			throw;
		}

		return objToReturn;
	}
}
