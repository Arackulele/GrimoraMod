using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class AssetUtils
{
	private static readonly Dictionary<string, string> FileChecksums = new()
	{
		{ "grimoramod_abilities", "B0C80AFA770389594F6454A90A29FB5C943EDA5E10F51852B1C51183DA9361EB" },
		{ "grimoramod_controller", "74BC4A80C0FA64CF5EF3F578DCB49625DBA54079785C210E7B2DC79B87C86FC5" },
		{ "grimoramod_mats", "939AD534C55F4150F586CE706345E19E80ABA39EC1D4298A61192220B3B1790F" },
		{ "grimoramod_prefabs", "683DB17E991683778343B38474207BED2507F3276E494E69623421C288F51254" },
		{ "grimoramod_sounds", "A2FC231C491780A59F925C4F3FB83B2E789DE4323516C74FE148ADCC4549E1D2" },
		{ "grimoramod_sprites", "7EB38FE040C5A92D0395A85786D48647501B0B7CE264F9CBBBAB7721113C6000" },
	};

	private static string ValidateFile(string assetBundleFile)
	{
		string fileToRead = FileUtils.FindFileInPluginDir(assetBundleFile);
		using var sha256 = SHA256.Create();
		using var stream = File.OpenRead(fileToRead);
		byte[] checksum = sha256.ComputeHash(stream);
		var sha265Checksum = BitConverter.ToString(checksum).Replace("-", string.Empty);
		if (FileChecksums.TryGetValue(Path.GetFileName(fileToRead), out string correctChecksum) && correctChecksum != sha265Checksum)
		{
			Log.LogError($"[AssetUtils] File [{Path.GetFileName(fileToRead)}] checksum [{sha265Checksum}] does not match the correct one [{correctChecksum}]");
		}

		return fileToRead;
	}

	public static List<T> LoadAssetBundle<T>(string assetBundleFile) where T : UnityEngine.Object
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		string fileToRead = ValidateFile(assetBundleFile);
		AssetBundle assetBundle = AssetBundle.LoadFromFile(fileToRead);
		var loadedBundle = assetBundle.LoadAllAssets<T>();
		// GrimoraPlugin.Log.LogDebug($"Bundle [{assetBundle}] - {string.Join(",", loadedBundle.Select(_ => _.name))}");
		assetBundle.Unload(false);
		stopwatch.Stop();
		Log.LogDebug($"Time taken to load bundle [{assetBundleFile}]: [{stopwatch.ElapsedMilliseconds}]ms");
		return loadedBundle.ToList();
	}

	public static IEnumerator LoadAssetBundleAsync<T>(string assetBundleFile) where T : UnityEngine.Object
	{
		Stopwatch stopwatch = Stopwatch.StartNew();

		Type type = typeof(T);
		string fileToRead = ValidateFile(assetBundleFile);
		var bundleLoadRequest = AssetBundle.LoadFromFileAsync(fileToRead);
		yield return bundleLoadRequest;
		var bundle = bundleLoadRequest.assetBundle;
		var allAssetsRequest = bundle.LoadAllAssetsAsync<T>();
		yield return allAssetsRequest;
		// Log.LogDebug($"Bundle [{bundle}] - {bundle.GetAllAssetNames().Join()}");
		if (type == typeof(AudioClip))
		{
			AllSounds = allAssetsRequest.allAssets.Cast<AudioClip>().ToList();
		}
		else if (type == typeof(Material))
		{
			AllMats = allAssetsRequest.allAssets.Cast<Material>().ToList();
		}
		else if (type == typeof(GameObject))
		{
			AllPrefabs = allAssetsRequest.allAssets.Cast<GameObject>().ToList();
		}
		else if (type == typeof(RuntimeAnimatorController))
		{
			AllControllers = allAssetsRequest.allAssets.Cast<RuntimeAnimatorController>().ToList();
		}
		else if (type == typeof(Sprite))
		{
			AllSprites = allAssetsRequest.allAssets.Cast<Sprite>().ToList();
		}
		else if (type == typeof(Texture))
		{
			AllAbilitiesTextures = allAssetsRequest.allAssets.Cast<Texture>().ToList();
		}

		bundle.Unload(false);
		stopwatch.Stop();
		Log.LogDebug($"Time taken to load bundle [{assetBundleFile}]: [{stopwatch.ElapsedMilliseconds}]ms");
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
			if (type == typeof(AudioClip))
			{
				objToReturn = AllSounds.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
			else if (type == typeof(Material))
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
				objToReturn = AllAbilitiesTextures.Single(go => NameMatchesAsset(go, prefabName)) as T;
			}
		}
		catch (Exception e)
		{
			Log.LogError(
				$"Unable to find prefab [{prefabName}]! This could mean the asset bundle doesn't contain it, or, most likely, your mod manager didn't correctly update the asset bundle files."
			+ "If it worked last update, delete your files and download the mod files again. "
			+ "There's a weird issue with how mod managers handle asset bundle between mod updates."
			);
			throw;
		}

		return objToReturn;
	}
}
