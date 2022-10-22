namespace JYJFramework
{
    public interface IState
    {
        void OnEnter();

        void OnUpdate();

        void OnExit();
    }
}

