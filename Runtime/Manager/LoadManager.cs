using System;
using System.Collections;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace JFramework
{
    public class LoadManager : Singleton<LoadManager>
    {
        public void LoadScene(string name) => SceneManager.LoadScene(name);

        public void LoadSceneAsync(string name, Action action = null)
        {
            MonoManager.Instance.StartCoroutine(LoadSceneCompleted(name, action));
        }

        private IEnumerator LoadSceneCompleted(string name, Action action = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            if (asyncOperation == null) yield break;
            while (!asyncOperation.isDone)
            {
                EventManager.Instance.Send("LoadSceneAsync", asyncOperation.progress);
                yield return asyncOperation;
            }

            action?.Invoke();
        }
    }
}