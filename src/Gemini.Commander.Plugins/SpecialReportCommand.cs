using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;

namespace Gemini.Commander.Plugins
{
    [Command("show", "special-report", Usage = "log show special-report")]
    public class SpecialReportCommand : ServiceManagerCommand
    {
        public SpecialReportCommand(ServiceManager svc) : base(svc)
        {
        }

        public override void Execute(MainArgs args)
        {
            Console.WriteLine("Special report");
        }
    }
}
