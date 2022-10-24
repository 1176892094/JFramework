using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using AssetDatabase = UnityEditor.AssetDatabase;
using Debug = UnityEngine.Debug;
using File = System.IO.File;

namespace JYJFramework
{
    public class FrameworkEditor
    {
        [MenuItem("Tools/JYJFramework/AssetsPath")]
        private static void AssetsPath()
        {
            Process.Start(Application.dataPath);
        }

        [MenuItem("Tools/JYJFramework/PersistentPath")]
        private static void PersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/JYJFramework/StreamingAssetsPath")]
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
    }
}