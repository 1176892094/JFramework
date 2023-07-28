namespace JFramework.Interface
{
    public interface IPanel : IEntity
    {
        UIStateType state { get; }
        
        void Show();

        void Hide();
    }
}