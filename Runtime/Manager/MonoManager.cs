using System;
using System.Collections;
using UnityEngine;

namespace JFramework
{
    public class MonoManager : Singleton<MonoManager>
    {
        private readonly MonoController controller;

        public MonoManager()
        {
            GameObject obj = new GameObject("MonoController");
            controller = obj.AddComponent<MonoController>();
            obj.hideFlags = HideFlags.HideAndDontSave;
        }

        public void Listen(Action action) => controller.Listen(action);

        public void Remove(Action action) => controller.Remove(action);

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return controller.StartCoroutine(coroutine);
        }

        public void StopCoroutine(IEnumerator coroutine) => controller.StopCoroutine(coroutine);

        public void StopCoroutines() => controller.StopAllCoroutines();
    }
    
    internal class MonoController : MonoBehaviour
    {
        private event Action UpdateAction;
        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Update() => UpdateAction?.Invoke();
        public void Listen(Action action) => UpdateAction += action;
        public void Remove(Action action) => UpdateAction -= action;
    }
}