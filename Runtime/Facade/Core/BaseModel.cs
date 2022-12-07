namespace JFramework.Basic
{
    public abstract class BaseModel : IModel
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

        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }
    
    public interface IModel : IGetFacade, ISetFacade, IGetExtend
    {
        void Init();
    }
}