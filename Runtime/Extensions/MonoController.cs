using System;
using UnityEngine;

namespace JFramework.Basic
{
    internal class MonoController: MonoBehaviour
    {
        private event Action UpdateAction;
        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Update() => UpdateAction?.Invoke();

        public void AddEventListener(Action action) => UpdateAction += action;

        public void RemoveEventListener(Action action) => UpdateAction -= action;
    }
}