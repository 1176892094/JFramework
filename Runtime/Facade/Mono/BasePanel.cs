namespace JFramework.Basic
{
    public abstract class BasePanel : BaseBehaviour, IPanel
    {
        public string Path { get; set; }
        
        protected abstract override void OnUpdate();
        
        public abstract void Show();

        public abstract void Hide();
    }
    
    public interface IPanel
    {
        string Path { set; }
        void Show();
        void Hide();
    }
}