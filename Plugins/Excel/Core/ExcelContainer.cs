using JFramework.Basic;
using UnityEngine;

namespace JFramework.Excel
{
    public abstract class ExcelContainer : ScriptableObject, IData
    {
        public abstract void InitData();
        public virtual void SaveData() => JsonManager.Save(this, name);
        public virtual void LoadData() => JsonManager.Load(this);

        public abstract void AddData(ExcelData data);
        public abstract ExcelData GetData(int index);
        public abstract int GetCount();
    }
}