namespace JFramework.Basic
{
    public abstract class BaseCommand : ICommand
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

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }
    
    public interface ICommand : IGetFacade, ISetFacade, IGetSystem, IGetModel, IGetExtend, ISendCommand, ISendQuery
    {
        void Execute();
    }
}