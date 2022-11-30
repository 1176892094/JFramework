namespace JFramework.Basic
{
    public class PlayerData: BaseData
    {
        public override void SaveData() => JsonManager.Save(this, name,true);

        public override void LoadData() => JsonManager.Load(this,true);
    }
}