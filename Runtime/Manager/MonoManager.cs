using System;
using System.Collections;
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

        public void AddEventListener(Action action) => controller.AddEventListener(action);

        public void RemoveEventListener(Action action) => controller.RemoveEventListener(action);
        
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return controller.StartCoroutine(coroutine);
        }
    }
}