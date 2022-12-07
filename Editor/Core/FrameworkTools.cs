using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Directory = System.IO.Directory;

namespace JFramework.Editor
{
    internal static class FrameworkTools
    {
        private struct DirectoryType
        {
            private const string Android = "/Extensions/Android";
            private const string Windows = "/Extensions/Windows";
            private const string WebGL = "/Extensions/WebGL";
            private const string iOS = "/Extensions/iOS";
            private const string Mac = "/Extensions/Mac";
            private const string Audios = "/Resources/Audios";
            private const string Models = "/Resources/Models";
            private const string Physics = "/Resources/Physics";
            private const string Prefabs = "/Resources/Prefabs";
            private const string Process = "/Resources/Process";
            private const string Animations = "/Animations";
            private const string Materials = "/Materials";
            private const string Scenes = "/Scenes";
            private const string Scripts = "/Scripts";
            private const string Settings = "/Settings";
            private const string Plugins = "/Plugins";
            private const string Shaders = "/Shaders";
            private const string Textures = "/Textures";
            private const string StreamingAssets = "/StreamingAssets";
            private const string Tilemaps = "/Tilemaps";

            public static readonly string[] DirectoryArray = new[]
            {
                Android, Windows, WebGL, iOS, Mac, Audios,
                Settings, Physics, Models, Textures, Prefabs,
                Materials, Animations, Plugins, Scenes, Scripts,
                Shaders, Process, Tilemaps, StreamingAssets
            };
        }

        [MenuItem("Tools/JFramework/CurrentProjectPath")]
        private static void ProjectFilePath()
        {
            Process.Start(System.Environment.CurrentDirectory);
        }

        [MenuItem("Tools/JFramework/PersistentDataPath")]
        private static void PersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Tools/JFramework/StreamingAssetsPath")]
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

        [MenuItem("Tools/JFramework/Initialization", false, 11)]
        private static void Initialization()
        {
            foreach (var directory in DirectoryType.DirectoryArray)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(Application.dataPath + directory);
                }
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
            File.Copy(origin, target, false);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}