namespace JYJFramework
{
    public class PlayerBaseData: BaseData
    {
        public override void SaveData() => JsonManager.SaveJson(this, name,true);

        public override void LoadData() => JsonManager.LoadJson(this,true);
    }
}