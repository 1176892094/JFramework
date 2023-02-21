namespace JFramework
{
    public static class DataExtension
    {
        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">数据自身</param>
        /// <typeparam name="T">可以转换继承IData的对象</typeparam>
        /// <returns>返回目标数据</returns>
        public static T As<T>(this IData data) where T : IData => (T)data;

        /// <summary>
        /// string转string
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出string类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out string result) => string.IsNullOrEmpty(result = reword);

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出int类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out int result) => int.TryParse(reword, out result);

        /// <summary>
        /// string转float
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出float类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out float result) => float.TryParse(reword, out result);

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出double类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out double result) => double.TryParse(reword, out result);

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出bool类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out bool result) => bool.TryParse(reword, out result);

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出long类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out long result) => long.TryParse(reword, out result);
    }
}