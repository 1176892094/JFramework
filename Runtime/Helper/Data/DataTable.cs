using JFramework.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 数据表抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataTable<T> : ScriptableObject, IDataTable where T : IData
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        [SerializeField] private List<T> dataList = new List<T>();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data"></param>
        public void AddData(T data) => dataList.Add(data);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetData(int index) => dataList[index];

        /// <summary>
        /// 数据的数量
        /// </summary>
        int IDataTable.Count => dataList.Count;

        /// <summary>
        /// 接口的添加数据方法
        /// </summary>
        /// <param name="data"></param>
        void IDataTable.AddData(IData data) => AddData((T)data);

        /// <summary>
        /// 接口的获取数据方法
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IData IDataTable.GetData(int index) => GetData(index);
    }
}