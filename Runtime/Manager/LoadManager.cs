using System;
using System.Collections;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace JFramework
{
    public static class LoadManager
    {
        public static void LoadScene(string name) => SceneManager.LoadScene(name);

        public static void LoadSceneAsync(string name, Action action = null)
        {
            MonoManager.Instance.StartCoroutine(LoadSceneCompleted(name, action));
        }

        private static IEnumerator LoadSceneCompleted(string name, Action action = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            if (asyncOperation == null) yield break;
            while (!asyncOperation.isDone)
            {
                EventManager.OnTrigger("LoadSceneAsync", asyncOperation.progress);
                yield return asyncOperation;
            }
            action?.Invoke();
        }
    }
}