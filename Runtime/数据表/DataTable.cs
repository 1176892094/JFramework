using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public interface IData { }

    public class KeyAttribute : Attribute { }

    public interface IDataTable
    {
        int Count { get; }

        void AddData(object data);

        object GetData(int index);
    }

    public class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        [SerializeField] private List<T> dataList = new List<T>();
        private void AddData(T data) => dataList.Add(data);
        private T GetData(int index) => dataList[index];
        int IDataTable.Count => dataList.Count;
        void IDataTable.AddData(object data) => AddData((T)data);
        object IDataTable.GetData(int index) => GetData(index);
    }
}
