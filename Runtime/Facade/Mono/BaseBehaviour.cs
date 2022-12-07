using System;
using UnityEngine;

namespace JFramework.Basic
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (Logger.LogLevel != LogLevel.Lowest)
            {
                Logger.Log(gameObject.name + "添加OnUpdate侦听");
                EventManager.Instance.Send("AddUpdate",gameObject.name);
            }

            MonoManager.Instance.Listen(OnUpdate);
        }

        [Obsolete("[JFramework] 请使用OnUpdate来代替Update管理生命周期", true)]
        protected void Update()
        {
        }
        
        protected abstract void OnUpdate();
        
        protected virtual void OnDestroy()
        {
            MonoManager.Instance.Remove(OnUpdate);
            if (Logger.LogLevel != LogLevel.Lowest)
            {
                Logger.Log(gameObject.name + "移除OnUpdate侦听");
                EventManager.Instance.Send("RemoveUpdate",gameObject.name);
            }
        }
    }
}