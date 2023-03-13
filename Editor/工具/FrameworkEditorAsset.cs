using System;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace JFramework
{
    internal class FrameworkEditorAsset : DataSingleton<FrameworkEditorAsset>
    {
        private AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
        [ShowInInspector,LabelText("文件夹路径")] private string[] pathList;
        [ShowInInspector,LabelText("文件夹名称")] private string[] nameList;
        private const string filter = "t:Object t:prefab";
        
        [Button("Addressable资源生成")]
        public void LoadAddressableGroup()
        {
            pathList = Directory.GetDirectories("Assets/" + Const.AddressableResources);
            nameList = pathList.Select(dir => dir.Split('/')).Select(strArray => strArray[^1]).ToArray();

            for (int i = 0; i < nameList.Length; i++)
            {
                var index = i;
                GetAddressableGroup(nameList[i], pathList[i], filter, assetPath =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(assetPath);
                    string dirPath = pathList[index];
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

            Debug.Log($"可寻址资源文件夹: {name.Orange()} 资源文件数量: {assets.Length.ToString().Green()}");
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
            AddressableAssetEntry entry = group.entries.LastOrDefault(e => e.guid == guid) ??
                                          Settings.CreateOrMoveEntry(guid, group, false, false);
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