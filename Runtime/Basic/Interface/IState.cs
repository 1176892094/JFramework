namespace JFramework.Basic
{
    public interface IState
    {
        void OnEnter();
        
        void OnUpdate();

        void OnExit();
    }
}