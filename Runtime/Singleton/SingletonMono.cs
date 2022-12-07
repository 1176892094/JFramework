using System;
using JFramework.Basic;
using UnityEngine;

public class SingletonMono<T> : BaseBehaviour where T : SingletonMono<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = (T)FindObjectOfType(typeof(T));
            return instance;
        }
    }

    protected override void OnUpdate()
    {
        
    }
}