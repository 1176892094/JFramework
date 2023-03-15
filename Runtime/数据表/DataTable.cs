using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        [SerializeField] private List<T> dataList = new List<T>();
        public void AddData(T data) => dataList.Add(data);
        public T GetData(int index) => dataList[index];
        int IDataTable.Count => dataList.Count;
        void IDataTable.AddData(object data) => AddData((T)data);
        object IDataTable.GetData(int index) => GetData(index);
    }
    
    public class KeyAttribute : Attribute { }
}
