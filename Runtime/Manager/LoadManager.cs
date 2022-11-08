using System;
using System.Collections;
using JFramework.Async;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace JFramework
{
    public static class LoadManager
    {
        public static void LoadScene(string name) => SceneManager.LoadScene(name);

        public static async void LoadSceneAsync(string name, Action action = null)
        {
            await LoadSceneCompleted(name, action);
        }

        private static IEnumerator LoadSceneCompleted(string name, Action action = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            if (asyncOperation == null) yield break;
            while (!asyncOperation.isDone)
            {
                EventManager.OnEventTrigger("LoadSceneAsync", asyncOperation.progress);
                yield return asyncOperation;
            }
            action?.Invoke();
        }
    }
}