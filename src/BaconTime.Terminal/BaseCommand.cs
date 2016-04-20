using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Countersoft.Gemini.Api;
using Fclp;

namespace BaconTime.Terminal
{
    public abstract class BaseCommand : ICommand
    {
        public abstract void Execute(MainArgs args);
    }

    public abstract class ServiceManagerCommand : BaseCommand
    {
        public ServiceManager Svc { get; set; }

        protected ServiceManagerCommand(ServiceManager svc)
        {
            Svc = svc;
        }
    }
}