namespace JFramework
{
    /// <summary>
    /// 字段
    /// </summary>
    internal abstract class Parser
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string name;

        /// <summary>
        /// 字段类型
        /// </summary>
        protected string type;

        /// <summary>
        /// 获取字段行
        /// </summary>
        /// <returns>返回一行字段</returns>
        public abstract string GetFieldLine();
    }
}