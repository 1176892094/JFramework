using System;
using System.Reflection;

namespace JFramework.Excel
{
	[Serializable]
	public abstract class ExcelData
	{
		public object GetKeyFieldValue()
		{
			FieldInfo keyField = ExcelExtend.GetDataKeyField(GetType());
			return keyField == null ? null : keyField.GetValue(this);
		}

		public abstract void InitData();
		
		protected bool TryParse(string raw, out string ret)
		{
			ret = raw;
			return true;
		}
		
		protected bool TryParse(string raw, out int ret)
		{
			return int.TryParse(raw, out ret);
		}
		
		protected bool TryParse(string raw, out float ret)
		{
			return float.TryParse(raw, out ret);
		}
		
		protected bool TryParse(string raw, out double ret)
		{
			return double.TryParse(raw, out ret);
		}
		
		protected bool TryParse(string raw, out bool ret)
		{
			return bool.TryParse(raw, out ret);
		}
		
		protected bool TryParse(string raw, out long ret)
		{
			return long.TryParse(raw, out ret);
		}
	}
}