using System;
using UnityEngine;

namespace JFramework.Excel
{
    internal class ExcelLoader
    {
        public ExcelContainer Load(string name)
        {
            int index = ExcelSetting.Instance.AssetPath.IndexOf("Resources/", StringComparison.Ordinal);
            string filePath = ExcelSetting.Instance.AssetPath.Substring(index + "Resources/".Length) + name;
            ExcelContainer table = (ExcelContainer)Resources.Load(filePath);
            return table;
        }
    }
}