// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 23:04:57
// // # Recently: 2025-04-09 23:04:57
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.IO;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    internal static partial class EditorSetting
    {
        [MenuItem("Tools/JFramework/更新 AB 资源", priority = 4)]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void UpdateAsset()
        {
            var assetBundles = AssetDatabase.GetAllAssetBundleNames();
            foreach (var assetBundle in assetBundles)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetBundle).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundle, true);
                }
            }

            GlobalSetting.Instance.sceneAssets.Clear();
            var folderPaths = AssetDatabase.GetSubFolders(GlobalSetting.Instance.assetSourcePath);
            foreach (var folderPath in folderPaths)
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    continue;
                }

                var folderName = Path.GetFileNameWithoutExtension(folderPath);
                var assetGuids = AssetDatabase.FindAssets("t:Object", new[] { folderPath });

                foreach (var assetGuid in assetGuids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                    if (AssetDatabase.IsValidFolder(assetPath))
                    {
                        continue;
                    }

                    var assetData = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (assetData == null)
                    {
                        continue;
                    }

                    if (assetData is DefaultAsset)
                    {
                        continue;
                    }

                    if (GlobalSetting.Instance.ignoreAssets.Contains(assetData))
                    {
                        continue;
                    }

                    if (assetData is SceneAsset)
                    {
                        GlobalSetting.Instance.sceneAssets.Add(assetPath);
                    }

                    var importer = AssetImporter.GetAtPath(assetPath);
                    if (importer.assetBundleName != folderName.ToLower())
                    {
                        importer.assetBundleName = folderName;
                        importer.SaveAndReimport();
                        Debug.Log(Service.Text.Format("增加AB资源: {0}", assetPath));
                    }

                    GlobalManager.assetPath[Service.Text.Format("{0}/{1}", folderName, assetData.name)] = assetPath;
                }
            }

            AssetDatabase.Refresh();
        }
    }
}