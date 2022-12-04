using System;
using UnityEngine;

namespace JFramework.Basic
{
    internal class MonoController: MonoBehaviour
    {
        private event Action UpdateAction;
        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Update() => UpdateAction?.Invoke();

        public void AddListener(Action action) => UpdateAction += action;

        public void RemoveListener(Action action) => UpdateAction -= action;
    }
}