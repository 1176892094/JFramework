// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Core;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        [SerializeField] private List<T> dataList = new List<T>();

        public int Count => dataList.Count;

        public void AddData(T data) => dataList.Add(data);

        public T GetData(int index) => dataList[index];

        public void Clear() => dataList.Clear();

        void IDataTable.AddData(IData data) => AddData((T)data);

        IData IDataTable.GetData(int index) => GetData(index);
    }

    [Serializable]
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Awake() => this.Inject();

        protected virtual void OnDestroy() => EntityManager.Destroy(this);
    }

    [Serializable]
    public abstract class UIPanel : MonoBehaviour, IEntity
    {
        public UILayer layer = UILayer.Normal;

        public UIState state = UIState.Common;

        protected virtual void Awake() => this.Inject();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }

    [Serializable]
    public abstract class Component<T> : ScriptableObject, IComponent where T : IEntity
    {
        [SerializeField] private T instance;

        public T owner => instance ??= (T)EntityManager.instance;

        void IComponent.OnAwake(IEntity instance) => this.instance ??= (T)instance;
    }

    [Serializable]
    public abstract class AttributeComponent<T1, T2> : Component<T1> where T1 : IEntity where T2 : Enum
    {
        private readonly Dictionary<T2, SecretFloat> attributes = new Dictionary<T2, SecretFloat>();

        public float Get(T2 key)
        {
            attributes.TryAdd(key, 0);
            return attributes[key];
        }

        public void Set(T2 key, float value)
        {
            attributes.TryAdd(key, 0);
            attributes[key] = value;
        }

        protected virtual void OnDestroy()
        {
            attributes.Clear();
        }
    }

    [Serializable]
    public abstract class State<T> : IState where T : IEntity
    {
        public T owner { get; private set; }

        protected abstract void OnEnter();

        protected abstract void OnUpdate();

        protected abstract void OnExit();

        void IState.OnAwake(IEntity owner) => this.owner = (T)owner;

        void IState.OnEnter() => OnEnter();

        void IState.OnUpdate() => OnUpdate();

        void IState.OnExit() => OnExit();
    }

    [Serializable]
    public abstract class StateMachine<T1> : Component<T1>, IStateMachine where T1 : IEntity
    {
        [ShowInInspector] private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        [ShowInInspector] private IState state;

        public void OnUpdate() => state?.OnUpdate();

        public bool IsActive<T2>() where T2 : IState
        {
            return state?.GetType() == typeof(T2);
        }

        public void AddState<T2>() where T2 : IState, new()
        {
            var newState = PoolManager.Dequeue<IState>(typeof(T2));
            states[typeof(T2)] = newState;
            newState.OnAwake(owner);
        }

        public void AddState<T2>(Type type) where T2 : IState
        {
            var newState = PoolManager.Dequeue<IState>(type);
            states[typeof(T2)] = newState;
            newState.OnAwake(owner);
        }

        public void ChangeState<T2>() where T2 : IState
        {
            if (!SceneManager.isLoading)
            {
                if (owner.gameObject.activeInHierarchy)
                {
                    state?.OnExit();
                    state = states[typeof(T2)];
                    state?.OnEnter();
                }
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var value in states.Values)
            {
                PoolManager.Enqueue(value, value.GetType());
            }

            states.Clear();
        }
    }
}