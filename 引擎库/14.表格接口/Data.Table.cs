// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 17:01:14
// # Recently: 2025-01-08 17:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        [SerializeField] private List<T> dataList = new List<T>();

        public int Count => dataList.Count;

        void IDataTable.AddData(IData data) => AddData((T)data);

        IData IDataTable.GetData(int index) => GetData(index);

        public void AddData(T data) => dataList.Add(data);

        public T GetData(int index) => dataList[index];
    }
}