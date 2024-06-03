// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:18
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework.Interface
{
    public interface IEntity
    {
        Transform transform { get; }

        GameObject gameObject { get; }
    }

    public interface IComponent
    {
        void OnAwake(IEntity owner);
    }

    public interface IState
    {
        void OnAwake(IEntity owner);

        void OnEnter();

        void OnUpdate();

        void OnExit();
    }

    public interface IStateMachine
    {
        bool IsActive<T>() where T : IState;

        void AddState<T>() where T : IState, new();

        void AddState<T>(Type type) where T : IState;

        void ChangeState<T>() where T : IState;
    }

    public interface IEvent
    {
    }

    public interface IEvent<in T> : IEvent where T : struct, IEvent
    {
        void Execute(T message);
    }

    public interface IPool : IDisposable
    {
        int count { get; }
    }

    public interface IPool<T> : IPool
    {
        T Pop();

        bool Push(T obj);
    }

    public interface IData
    {
    }

    public interface IDataTable
    {
        int Count { get; }

        void AddData(IData data);

        IData GetData(int index);

        void Clear();
    }
    
    public interface IGrid<T> : IEntity, IDisposable
    {
        T item { get; }

        void SetItem(T item);

        void Select();
    }
}