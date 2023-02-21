namespace JFramework
{
	/// <summary>
	/// 解析的抽象类
	/// </summary>
	internal abstract class Parse
	{
		/// <summary>
		/// 解析字段行
		/// </summary>
		/// <returns>返回字段行</returns>
		public abstract string GetFieldLine();

		/// <summary>
		/// 解析转换行
		/// </summary>
		/// <returns>返回转换行</returns>
		public abstract string GetParseLine();

		/// <summary>
		/// 解析初始化行
		/// </summary>
		/// <returns>返回初始化行</returns>
		public abstract string GetInitLine();
	}
}