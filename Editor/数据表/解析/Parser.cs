namespace JFramework
{
    /// <summary>
    /// 解析器的抽象类
    /// </summary>
    internal abstract class Parser : IParser
    {
        /// <summary>
        /// 解析字段名称
        /// </summary>
        public string name;

        /// <summary>
        /// 解析字段类型
        /// </summary>
        protected string type;

        /// <summary>
        /// 获取解析字段
        /// </summary>
        public abstract string GetField();

        /// <summary>
        /// 获取解析数据
        /// </summary>
        public abstract string GetParse();
    }
}