// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-10 00:04:55
// // # Recently: 2025-04-10 00:04:05
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static class DllCopyPostprocessor
{
    private static readonly List<string> assemblies = new List<string>()
    {
        "JFramework.dll",
        "JFramework.Kcp.dll"
    };

    static DllCopyPostprocessor()
    {
        CompilationPipeline.compilationFinished += OnCompilationFinished;
    }

    
    private static void OnCompilationFinished(object obj)
    {
        foreach (var assembly in assemblies)
        {
            var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "Library/ScriptAssemblies/" + assembly);
            var targetPath = Path.Combine("/Users/charlotte/Documents/GitHub/JFramework-Net", assembly);
            try
            {
                if (File.Exists(sourcePath))
                {
                    if (Directory.Exists(Path.GetDirectoryName(targetPath)))
                    {
                        File.Copy(sourcePath, targetPath, true);
                        Debug.Log($"程序集已复制到：{targetPath}");
                    }
                    else
                    {
                        Debug.LogWarning($"目标路径未找到：{targetPath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"程序集未找到：{sourcePath}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"程序集复制失败: {e.Message}");
            }
        }
    }
}