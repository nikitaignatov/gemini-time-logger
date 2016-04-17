using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fclp;

namespace BaconTime.Terminal
{
    public abstract class BaseCommand : ICommand
    {
        public abstract void Execute(string[] args);

        protected void ValidateParams(ICommandLineParserResult result)
        {
            if (result.HasErrors) throw new ArgumentException(result.ErrorText);
        }
    }
}