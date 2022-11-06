#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Directory = System.IO.Directory;

namespace JYJFramework.Editor
{
    public class FrameworkEditor
    {
        private struct DirectoryType
        {
            private const string Android = "/Extensions/Android";
            private const string Windows = "/Extensions/Windows";
            private const string WebGL = "/Extensions/WebGL";
            private const string iOS = "/Extensions/iOS";
            private const string Mac = "/Extensions/Mac";
            private const string Physics = "/Resources/Physics";
            private const string Meshes = "/Resources/Meshes";
            private const string Textures = "/Resources/Textures";
            private const string Sprites = "/Resources/Sprites";
            private const string Prefabs = "/Resources/Prefabs";
            private const string Audio = "/Resources/Audio";
            private const string Data = "/Resources/Data";
            private const string Editor = "/Editor";
            private const string Materials = "/Materials";
            private const string Animations = "/Animations";
            private const string Settings = "/Settings";
            private const string Plugins = "/Plugins";
            private const string Scenes = "/Scenes";
            private const string Scripts = "/Scripts";
            private const string Shaders = "/Shaders";
            private const string Flares = "/Flares";
            private const string Tilemaps = "/Tilemaps";
            private const string StreamingAssets = "/StreamingAssets";

            public static readonly string[] DirectoryArray = new[]
            {
                Android, Windows, WebGL, iOS, Mac, Settings, 
                Physics, Meshes, Textures, Sprites, Prefabs, 
                Audio, Data, Editor, Materials, Animations, 
                Plugins, Scenes, Scripts, Shaders, Flares, 
                Tilemaps, StreamingAssets
            };
        }
        
        [MenuItem("Tools/Framework/AssetsPath")]
        private static void AssetsPath()
        {
            Process.Start(Application.dataPath);
        }

        [MenuItem("Tools/Framework/PersistentPath")]
        private static void PersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/Framework/StreamingAssetsPath")]
        private static void StreamingAssetsPath()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Process.Start(Application.streamingAssetsPath);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                Process.Start(Application.streamingAssetsPath);
            }
        }
        
        [MenuItem("Tools/Framework/Initialization",false,11)]
        private static void Initialization()
        {
            foreach (var directory in DirectoryType.DirectoryArray)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(Application.dataPath + directory);
                }
            }
            if (!Directory.Exists(Application.dataPath + "/Resources/Settings/Json"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/Settings");
                CreateJsonData();
            }
            else if (!File.Exists(Application.dataPath + "Resources/Settings/JsonData.asset"))
            {
                CreateJsonData();
            }

            if (!File.Exists(Application.dataPath + "/Scenes/StartScene.unity"))
            {
                CreateScene();
            }
        }

        private static void CreateScene()
        {
            SceneAsset scene = ResourceManager.Load<SceneAsset>("StartScene");
            string origin = AssetDatabase.GetAssetOrScenePath(scene);
            string target = Application.dataPath + "/Scenes/StartScene.unity";
            File.Copy(origin,target,false);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateJsonData()
        {
            JsonData jsonData = ScriptableObject.CreateInstance<JsonData>();
            AssetDatabase.CreateAsset(jsonData,"Assets/Resources/Settings/JsonData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = jsonData;
        }
    }
}
#endif