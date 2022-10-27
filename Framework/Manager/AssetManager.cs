using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : SingletonMonoAuto<AssetManager>
{
    private AssetBundle asset;
    private AssetBundleManifest manifest;
    private readonly Dictionary<string, AssetBundle> assetDict = new Dictionary<string, AssetBundle>();
    private string PathURL => Application.streamingAssetsPath + "/";

    private string MainName
    {
        get
        {
#if UNITY_IOS
            return "IOS"
#elif UNITY_ANDROID
            return "Android"
#else
            return "PC";

#endif
        }
    }

    public Object Load(string packageName, string assetName)
    {
        if (asset == null)
        {
            asset = AssetBundle.LoadFromFile(PathURL + MainName);
            manifest = asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        AssetBundle assetBundle;
        string[] strArray = manifest.GetAllDependencies(packageName);
        foreach (var package in strArray)
        {
            if (!assetDict.ContainsKey(package))
            {
                assetBundle = AssetBundle.LoadFromFile(PathURL + package);
                assetDict.Add(package, assetBundle);
            }
        }
        
        if (!assetDict.ContainsKey(packageName))
        {
            assetBundle = AssetBundle.LoadFromFile(PathURL + packageName);
            assetDict.Add(packageName, assetBundle);
        }

        return assetDict[packageName].LoadAsset(assetName);
    }
}
