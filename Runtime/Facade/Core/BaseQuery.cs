namespace JFramework.Basic
{
    public abstract class BaseQuery<T> : IQuery<T>
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

        T IQuery<T>.Find()
        {
            return OnFind();
        }

        protected abstract T OnFind();
    }
    
    public interface IQuery<T> : IGetFacade, ISetFacade, IGetModel, IGetSystem, ISendQuery
    {
        T Find();
    }
}