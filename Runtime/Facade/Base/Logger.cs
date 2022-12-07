using UnityEngine;

namespace JFramework.Basic
{
    public static class Logger
    {
        public static LogLevel LogLevel = LogLevel.Lowest;
        
        public static void Log(string message)
        {
            Debug.Log("[JFramework] " + message);
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning("[JFramework] " + message);
        }

        public static void LogError(string message)
        {
            Debug.LogError("[JFramework] " + message);
        }
    }

    public enum LogLevel
    {
        Lowest,
        Middle,
        Height
    }
    
    public enum LanguageType
    {
        Chinese,
        English
    }
}