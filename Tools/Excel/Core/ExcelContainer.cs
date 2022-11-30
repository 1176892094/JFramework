using UnityEngine;

namespace JFramework.Excel
{
    public abstract class ExcelContainer : ScriptableObject
    {
        public string ExcelFileName;
        public abstract void InitData();
        public abstract void AddData(ExcelData data);
        public abstract ExcelData GetData(int index);
        public abstract int GetCount();
    }
}