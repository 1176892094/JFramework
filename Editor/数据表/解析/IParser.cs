namespace JFramework
{
    /// <summary>
    /// 解析器接口
    /// </summary>
    internal interface IParser
    {
        /// <summary>
        /// 获取字段
        /// </summary>
        string GetField();
        
        /// <summary>
        /// 获取解析
        /// </summary>
        string GetParse();
    }
}