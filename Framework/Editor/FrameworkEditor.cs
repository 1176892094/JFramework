using System.IO;
using UnityEditor;
using UnityEngine;
using AssetDatabase = UnityEditor.AssetDatabase;

namespace JYJFramework
{
    public class FrameworkEditor
    {
        [MenuItem("Tools/JYJFramework/AssetsPath")]
        private static void AssetsPath()
        {
            EditorUtility.OpenFolderPanel("JYJFramework", Application.dataPath,"");
        }

        [MenuItem("Tools/JYJFramework/PersistentPath")]
        private static void PersistentDataPath()
        {
            EditorUtility.OpenFolderPanel("JYJFramework", Application.persistentDataPath,"");
        }
        
        [MenuItem("Tools/JYJFramework/StreamingAssetsPath")]
        private static void StreamingAssetsPath()
        {
            EditorUtility.OpenFolderPanel("JYJFramework", Application.streamingAssetsPath,"");
        }
    }
}