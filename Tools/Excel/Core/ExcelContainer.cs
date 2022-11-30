using UnityEngine;

namespace JFramework.Excel
{
    public abstract class ExcelContainer : ScriptableObject
    {
        public string ExcelFileName;
        public abstract void AddData(ExcelData data);
        public abstract int GetCount();
        public abstract ExcelData GetData(int index);
        public abstract void InitData();
    }
}