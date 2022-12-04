using JFramework.Basic;

namespace JFramework.Excel
{
    public abstract class ExcelContainer : BaseData
    {
        public abstract override void InitData();
        public override void SaveData() => JsonManager.Save(this, name);
        public override void LoadData() => JsonManager.Load(this);

        public abstract void AddData(ExcelData data);
        public abstract ExcelData GetData(int index);
        public abstract int GetCount();
    }
}