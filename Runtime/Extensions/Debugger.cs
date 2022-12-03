using UnityEngine;

namespace JFramework.Basic
{
    public static class Debugger
    {
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
}