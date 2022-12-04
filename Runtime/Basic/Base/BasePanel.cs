namespace JFramework.Basic
{
    public abstract class BasePanel : BaseBehaviour, IPanel
    {
        public string path;

        protected abstract override void OnUpdate();

        public abstract void Show();

        public abstract void Hide();
    }
}