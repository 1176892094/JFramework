// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:44
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        [SerializeField] private List<T> dataList = new List<T>();

        public void AddData(T data) => dataList.Add(data);

        public T GetData(int index) => dataList[index];

        int IDataTable.Count => dataList.Count;

        void IDataTable.AddData(IData data) => AddData((T)data);

        IData IDataTable.GetData(int index) => GetData(index);
    }

    [Serializable]
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Awake() => this.Inject();

        protected virtual void OnDestroy() => this.Destroy();
    }

    [Serializable]
    public abstract class UIPanel : MonoBehaviour, IPanel
    {
        public UILayer layer { get; protected set; } = UILayer.Normal;

        public UIState state { get; protected set; } = UIState.Common;

        protected virtual void Awake() => this.Inject();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }

    [Serializable]
    public abstract class Component<T> : ScriptableObject, IComponent where T : IEntity
    {
        [SerializeField] private T instance;

        public T owner => instance ??= (T)GlobalManager.Entity.instance;

        void IComponent.OnAwake(IEntity instance) => this.instance ??= (T)instance;
    }

    [Serializable]
    public abstract class AttributeComponent<T1, T2> : Component<T1> where T1 : IEntity where T2 : Enum
    {
        private readonly Dictionary<T2, IVariable> attributes = new Dictionary<T2, IVariable>();

        public T Get<T>(T2 key) where T : struct
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, new Variable<T>());
            }

            return ((Variable<T>)attributes[key]).value;
        }

        public void Set<T>(T2 key, T value) where T : struct
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, new Variable<T>());
            }

            ((Variable<T>)attributes[key]).value = value;
        }

        protected virtual void OnDestroy()
        {
            attributes.Clear();
        }
    }
}