using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace JFramework
{
    internal class FrameworkEditorHouse : DataSingleton<FrameworkEditorHouse>
    {
        private AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
        public string[] pathList;
        public List<string> nameList;

        [Button("Addressable资源生成")]
        public void LoadAddressableGroup()
        {
            pathList = Directory.GetDirectories("Assets/" + Const.AddressableResources);
            nameList = new List<string>();
            foreach (var dir in pathList)
            {
                string[] strArray = dir.Split('/');
                string str = strArray[^1];
                nameList.Add(str);
            }

            for (int i = 0; i < nameList.Count; i++)
            {
                var filter = "t:ScriptableObject t:prefab t:AudioClip t:Shader t:Material t:SceneAsset t:Texture2D";
                var path = i;
                GetAddressableGroup(nameList[i], pathList[i], filter, assetPath =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(assetPath);
                    string dirPath = pathList[path];
                    string dirName = Path.GetFileNameWithoutExtension(dirPath);
                    return $"{dirName}/{fileName}";
                });
            }
        }

        private void GetAddressableGroup(string name, string folder, string filter, Func<string, string> getAddress)
        {
            var assets = GetAssets(folder, filter);
            AddressableAssetGroup group = CreateGroup<AddressableAssetGroup>(name);

            foreach (var assetPath in assets)
            {
                string address = getAddress(assetPath);
                AddAssetEntry(group, assetPath, address);
            }

            Debug.Log($"重新生成Addressable资源组: {name} \r\n资源文件路径: {folder} \r\n资源文件数量: {assets.Length}");
        }

        private AddressableAssetGroup CreateGroup<T>(string groupName)
        {
            AddressableAssetGroup group = Settings.FindGroup(groupName);
            if (group == null)
            {
                group = Settings.CreateGroup(groupName, false, false, false, Settings.DefaultGroup.Schemas, typeof(T));
            }

            return group;
        }

        private void AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = group.entries.LastOrDefault(e => e.guid == guid) ?? Settings.CreateOrMoveEntry(guid, group, false, false);
            entry.address = address;
        }

        private string[] GetAssets(string folder, string filter)
        {
            if (folder.IsEmpty()) return null;
            if (filter.IsEmpty()) return null;
            folder = folder.TrimEnd('/').TrimEnd('\\');
            if (!filter.StartsWith("t:")) return null;
            var guids = AssetDatabase.FindAssets(filter, new[] { folder });
            var paths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return paths;
        }
    }
}