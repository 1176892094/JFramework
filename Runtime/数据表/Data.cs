using System;
using JFramework.Core;

namespace JFramework
{
    /// <summary>
    /// 基本数据的抽象类
    /// </summary>
    [Serializable]
    public abstract class Data
    {
        /// <summary>
        /// 获取数据的键值
        /// </summary>
        /// <returns>返回数据的主键</returns>
        public object KeyValue()
        {
            var key = DataManager.KeyValue(GetType());
            return key == null ? null : key.GetValue(this);
        }

        /// <summary>
        /// 数据对象初始化
        /// </summary>
        public abstract void InitData();

        /// <summary>
        /// string转string
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出string类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out string result)
        {
            result = reword;
            return true;
        }

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出int类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out int result)
        {
            return int.TryParse(reword, out result);
        }

        /// <summary>
        /// string转float
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出float类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out float result)
        {
            return float.TryParse(reword, out result);
        }

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出double类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out double result)
        {
            return double.TryParse(reword, out result);
        }

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出bool类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out bool result)
        {
            return bool.TryParse(reword, out result);
        }

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出long类型</param>
        /// <returns>返回改写是否成功</returns>
        protected bool TryParse(string reword, out long result)
        {
            return long.TryParse(reword, out result);
        }
    }
}