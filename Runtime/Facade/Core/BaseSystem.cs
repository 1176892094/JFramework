namespace JFramework.Basic
{
    public abstract class BaseSystem : ISystem
    {
        private IFacade facade;

        IFacade IGetFacade.GetFacade()
        {
            return facade;
        }

        void ISetFacade.SetFacade(IFacade facade)
        {
            this.facade = facade;
        }

        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }
    
    public interface ISystem : IGetFacade, ISetFacade, IGetModel, IGetExtend, IGetSystem
    {
        void Init();
    }
}