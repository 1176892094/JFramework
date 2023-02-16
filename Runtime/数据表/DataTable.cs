using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 数据表的抽象类
    /// </summary>
    public abstract class DataTable : ScriptableObject
    {
        /// <summary>
        /// 数据表获取列表中的数据总数
        /// </summary>
        /// <returns></returns>
        public abstract int Count { get; }
        /// <summary>
        /// 数据表初始化数据
        /// </summary>
        public abstract void InitData();

        /// <summary>
        /// 数据表添加数据
        /// </summary>
        /// <param name="data">传入添加的数据</param>
        public abstract void AddData(Data data);

        /// <summary>
        /// 数据表得到数据
        /// </summary>
        /// <param name="index">传入数据在列表中的位置</param>
        /// <returns>返回对应位置的数据</returns>
        public abstract Data GetData(int index);
    }
}