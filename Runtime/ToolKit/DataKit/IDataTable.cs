// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:05
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    /// <summary>
    /// 数据表接口
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 数据数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data"></param>
        void AddData(IData data);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IData GetData(int index);
    }
}