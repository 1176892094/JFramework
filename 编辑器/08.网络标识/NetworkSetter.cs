// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 00:01:56
// # Recently: 2025-01-09 00:01:57
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace JFramework
{
    internal static class NetworkSetter
    {
        private static readonly Dictionary<ulong, GameObject> caches = new Dictionary<ulong, GameObject>();

        private static KeyValuePair<string, ulong> Validate(string assetId, ulong sceneId, GameObject assetData)
        {
            var gameObject = assetData.gameObject;
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                sceneId = 0;
                assetId = AssignAssetPath(gameObject.name, AssetDatabase.GetAssetPath(gameObject));
            }
            else if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                if (PrefabStageUtility.GetPrefabStage(gameObject) != null)
                {
                    sceneId = 0;
                    assetId = AssignAssetPath(gameObject.name, PrefabStageUtility.GetPrefabStage(gameObject).assetPath);
                }
            }
            else if (IsSceneObjectWithPrefabParent(gameObject, out var prefab))
            {
                sceneId = AssignSceneId(assetData, sceneId);
                assetId = AssignAssetPath(gameObject.name, AssetDatabase.GetAssetPath(prefab));
            }
            else
            {
                sceneId = AssignSceneId(assetData, sceneId);
            }

            return new KeyValuePair<string, ulong>(assetId, sceneId);
        }

        private static string AssignAssetPath(string assetName, string assetPath)
        {
            if (!string.IsNullOrWhiteSpace(assetPath))
            {
                var importer = AssetImporter.GetAtPath(assetPath);
                if (importer != null)
                {
                    var asset = importer.assetBundleName;
                    return char.ToUpper(asset[0]) + asset.Substring(1) + "/" + assetName;
                }
            }

            return string.Empty;
        }

        private static bool IsSceneObjectWithPrefabParent(GameObject assetData, out GameObject prefab)
        {
            prefab = null;
            if (!PrefabUtility.IsPartOfPrefabInstance(assetData))
            {
                return false;
            }

            prefab = PrefabUtility.GetCorrespondingObjectFromSource(assetData);
            if (prefab != null)
            {
                return true;
            }

            Debug.LogError(Service.Text.Format("找不到场景对象的预制父物体。对象名称: {0}", assetData.name));
            return false;
        }

        private static ulong AssignSceneId(GameObject assetData, ulong sceneId)
        {
            if (Application.isPlaying) return sceneId;
            var duplicate = caches.TryGetValue(sceneId, out var @object) && @object != null && @object != assetData;
            if (sceneId == 0 || duplicate)
            {
                sceneId = 0;
                if (BuildPipeline.isBuildingPlayer)
                {
                    throw new InvalidOperationException("请保存场景后，再进行构建。");
                }

                Undo.RecordObject(assetData, "生成场景Id");
                var randomId = Service.Hash.Id();
                duplicate = caches.TryGetValue(randomId, out @object) && @object != null && @object != assetData;
                if (!duplicate)
                {
                    sceneId = randomId;
                }
            }

            caches[sceneId] = assetData;
            return sceneId;
        }
    }
}