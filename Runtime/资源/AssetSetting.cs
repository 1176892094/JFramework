using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    public class AssetSetting : AssetSingleton<AssetSetting>
    {
#if UNITY_EDITOR
        private AssetImporter importer;

        public void Update()
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (string assetBundleName in assetBundleNames)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }

            if (!Directory.Exists(AssetConst.FILE_PATH))
            {
                Directory.CreateDirectory(AssetConst.FILE_PATH);
            }
            
            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { AssetConst.FILE_PATH });
            var enumerable = guids.Select(AssetDatabase.GUIDToAssetPath).Where(asset => !AssetDatabase.IsValidFolder(asset));
            foreach (var path in enumerable)
            {
                var array = path.Replace('\\', '/').Split('/');
                importer = AssetImporter.GetAtPath(path);
                if (importer != null)
                {
                    var assetBundleName = importer.assetBundleName;
                    if (assetBundleName != array[2].ToLower())
                    {
                        importer.assetBundleName = array[2];
                        importer.SaveAndReimport();
                    }
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
        }

        private static string[] GetAssetsByGuid(string folder, string filter)
        {
            if (string.IsNullOrEmpty(folder)) return null;
            if (string.IsNullOrEmpty(filter)) return null;
            folder = folder.TrimEnd('/').TrimEnd('\\');
            if (!filter.StartsWith("t:")) return null;
            var guids = AssetDatabase.FindAssets(filter, new[] { folder });
            return guids.Select(AssetDatabase.GUIDToAssetPath).Where(path => !AssetDatabase.IsValidFolder(path))
                .ToArray();
        }
#endif
    }
}