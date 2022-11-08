namespace JFramework
{
    public interface IState
    {
        void OnEnter();

        void OnUpdate();

        void OnExit();
    }
}

