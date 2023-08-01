using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework
{
    public class AssetSetting : AssetSingleton<AssetSetting>
    {
        internal Dictionary<string, Object> assets = new Dictionary<string, Object>();
        [SerializeField] private List<string> keys = new List<string>();
        [SerializeField] private List<Object> values = new List<Object>();
#if UNITY_EDITOR
        private AssetImporter importer;

        public void Update()
        {
            keys.Clear();
            values.Clear();
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
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!AssetDatabase.IsValidFolder(path))
                {
                    string[] array = path.Replace('\\', '/').Split('/');
                    if (array.Length > 3)
                    {
                        importer = AssetImporter.GetAtPath(path);
                        keys.Add($"{array[2]}/{Path.GetFileNameWithoutExtension(path)}");
                        values.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
                        if (importer != null)
                        {
                            if (string.IsNullOrEmpty(importer.assetBundleVariant))
                            {
                                importer.assetBundleName = array[2];
                            }
                            else
                            {
                                importer.assetBundleVariant = array[2];
                            }

                            importer.SaveAndReimport();
                        }
                    }
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
        }
#endif
    }
}