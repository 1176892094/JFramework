namespace JFramework
{
	/// <summary>
	/// 解析的抽象类
	/// </summary>
	internal interface IParser
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
	}
}