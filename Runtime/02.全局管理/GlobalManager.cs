// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:16
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IEntity
    {
        public static GlobalManager Instance { get; private set; }

        public static event Action OnUpdate;

        public static event Action OnFixedUpdate;
        
        public static event Action OnLateUpdate;

        public static event Action OnCheat;

        private void Awake()
        {
            Instance = this;
            UIManager.Register();
            PoolManager.Register();
            InputManager.Register();
            AudioManager.Register();
            TimerManager.Register();
            TweenManager.Register();
        }

        private void Update()
        {
            try
            {
                OnUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void FixedUpdate()
        {
            try
            {
                OnFixedUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        
        private void LateUpdate()
        {
            try
            {
                OnLateUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void OnApplicationQuit()
        {
            try
            {
                Instance = null;
                UIManager.UnRegister();
                DataManager.UnRegister();
                PoolManager.UnRegister();
                InputManager.UnRegister();
                AssetManager.UnRegister();
                AudioManager.UnRegister();
                SceneManager.UnRegister();
                TweenManager.UnRegister();
                TimerManager.UnRegister();
                EventManager.UnRegister();
                EntityManager.UnRegister();
                BundleManager.UnRegister();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void OnDestroy()
        {
            OnCheat = null;
            OnUpdate = null;
            OnLateUpdate = null;
            OnFixedUpdate = null;
            GC.Collect();
        }

        public static void Cheat()
        {
            Debug.LogWarning("检查到作弊！");
            OnCheat?.Invoke();
        }
    }
}