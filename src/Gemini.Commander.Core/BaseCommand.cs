using Countersoft.Gemini.Api;

namespace Gemini.Commander.Core
{
    public abstract class BaseCommand : ICommand
    {
        public abstract void Execute(MainArgs args);
    }
    public abstract class BaseQuery<TResult> : IQuery<TResult>
    {
        public abstract TResult Execute(MainArgs args);
    }

    public abstract class ServiceManagerCommand : BaseCommand
    {
        public ServiceManager Svc { get; set; }

        protected ServiceManagerCommand(ServiceManager svc)
        {
            Svc = svc;
        }
    }

    public abstract class ServiceManagerQuery<R> : BaseQuery<R>
    {
        public ServiceManager Svc { get; set; }

        protected ServiceManagerQuery(ServiceManager svc)
        {
            Svc = svc;
        }
    }
}