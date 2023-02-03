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
    internal class FrameworkEditorHouse : EditorSingleton<FrameworkEditorHouse>
    {
        private static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
        public List<string> pathList;
        public List<string> nameList;

        [Button("Addressable资源生成")]
        private void LoadAddressableGroup()
        {
            pathList = GetDirectories("Assets/" + EditorConst.AddressableResources);
            nameList = new List<string>();
            foreach (var dir in pathList)
            {
                string[] strArray = dir.Split('/');
                string str = strArray[^1];
                nameList.Add(str);
            }

            for (int i = 1; i < nameList.Count; i++)
            {
                GetAddressableGroup(nameList[i], pathList[i], "t:DataTable t:prefab t:AudioClip t:Shader t:Material t:SceneAsset", assetPath =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(assetPath);
                    string dirPath = Path.GetDirectoryName(assetPath);
                    string dirName = Path.GetFileNameWithoutExtension(dirPath);
                    return $"{dirName}/{fileName}";
                });
            }
        }

        private List<string> GetDirectories(string rootPath)
        {
            var dirList = new List<string> { rootPath };
            for (int i = 0; i < dirList.Count; i++)
            {
                if (Directory.Exists(dirList[i]))
                {
                    dirList.AddRange(Directory.GetDirectories(dirList[i]));
                }
            }

            return dirList;
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

        private static AddressableAssetGroup CreateGroup<T>(string groupName)
        {
            AddressableAssetGroup group = Settings.FindGroup(groupName);
            if (group == null)
            {
                group = Settings.CreateGroup(groupName, false, false, false, Settings.DefaultGroup.Schemas, typeof(T));
            }

            return group;
        }

        private static void AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = group.entries.FirstOrDefault(e => e.guid == guid) ?? Settings.CreateOrMoveEntry(guid, group, false, false);
            entry.address = address;
        }

        private string[] GetAssets(string folder, string filter)
        {
            if (string.IsNullOrEmpty(folder)) return null;
            if (string.IsNullOrEmpty(filter)) return null;
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