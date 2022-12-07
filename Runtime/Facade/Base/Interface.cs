namespace JFramework.Basic
{
    public interface IController : IGetFacade, ISendCommand, IGetSystem, IGetModel, ISendQuery
    {
    }
    
    public interface IExtend
    {
    }

    public interface IGetFacade
    {
        IFacade GetFacade();
    }

    public interface ISetFacade
    {
        void SetFacade(IFacade facade);
    }

    public interface IGetModel : IGetFacade
    {
    }

    public interface IGetSystem : IGetFacade
    {
    }

    public interface IGetExtend : IGetFacade
    {
    }

    public interface ISendCommand : IGetFacade
    {
    }

    public interface ISendQuery : IGetFacade
    {
    }
}