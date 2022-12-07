using System.Collections.Generic;

namespace JFramework.Basic
{
    public abstract class BaseFacade<T> : IFacade where T : BaseFacade<T>, new()
    {
        private readonly HashSet<ISystem> systems = new HashSet<ISystem>();

        private readonly HashSet<IModel> models = new HashSet<IModel>();

        private readonly BaseContainer container = new BaseContainer();

        private static T instance;

        public static IFacade Instance
        {
            get
            {
                if (instance == null)
                {
                    InitFacade();
                }

                return instance;
            }
        }

        private bool IsInit;

        protected abstract void Init();

        private static void InitFacade()
        {
            if (instance == null)
            {
                instance = new T();
                instance.Init();

                foreach (var architectureModel in instance.models)
                {
                    architectureModel.Init();
                }

                instance.models.Clear();

                foreach (var architectureSystem in instance.systems)
                {
                    architectureSystem.Init();
                }

                instance.systems.Clear();

                instance.IsInit = true;
            }
        }


        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetFacade(this);
            container.Register(system);

            if (!IsInit)
            {
                systems.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetFacade(this);
            container.Register(model);

            if (!IsInit)
            {
                models.Add(model);
            }
            else
            {
                model.Init();
            }
        }

        public void RegisterExtend<TExtend>(TExtend extend) where TExtend : IExtend
        {
            container.Register(extend);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return container.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return container.Get<TModel>();
        }

        public TExtend GetExtend<TExtend>() where TExtend : class, IExtend
        {
            return container.Get<TExtend>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            ExecuteCommand(command);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetFacade(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return FindQuery(query);
        }

        protected virtual TResult FindQuery<TResult>(IQuery<TResult> query)
        {
            query.SetFacade(this);
            return query.Find();
        }
    }
    
    public interface IFacade
    {
        void RegisterSystem<T>(T system) where T : ISystem;

        void RegisterModel<T>(T model) where T : IModel;

        void RegisterExtend<T>(T extend) where T : IExtend;

        T GetSystem<T>() where T : class, ISystem;

        T GetModel<T>() where T : class, IModel;

        T GetExtend<T>() where T : class, IExtend;

        T SendQuery<T>(IQuery<T> query);

        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
    }
}