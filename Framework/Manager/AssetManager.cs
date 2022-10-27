using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : SingletonMonoAuto<AssetManager>
{
    private AssetBundle assetBundle;
    private AssetBundleManifest assetBundleManifest;
    private Dictionary<string, AssetBundle> assetBundleDict = new Dictionary<string, AssetBundle>();
    private string PathURL => Application.streamingAssetsPath + "/";

    private string MainPackageName
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

    public void LoadAsset(string package, string asset)
    {
        if (assetBundle == null)
        {
            assetBundle = AssetBundle.LoadFromFile(PathURL);
        }
    }
}
