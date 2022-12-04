using System;
using System.Collections;
using System.ComponentModel;
using JFramework.Basic;
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

        public void AddListener(Action action) => controller.AddListener(action);

        public void RemoveListener(Action action) => controller.RemoveListener(action);

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return controller.StartCoroutine(coroutine);
        }

        public void StopCoroutine(IEnumerator coroutine)
        {
            controller.StopCoroutine(coroutine);
        }

        public void StopAllCoroutines()
        {
            controller.StopAllCoroutines();
        }
    }
}