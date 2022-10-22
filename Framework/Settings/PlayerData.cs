namespace JYJFramework
{
    public class PlayerData: BaseData
    {
        public override void SaveData() => JsonManager.SaveJson(this, name,true);

        public override void LoadData() => JsonManager.LoadJson(this,true);
    }
}