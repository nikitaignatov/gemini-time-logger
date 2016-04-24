using System;
using System.Collections.Generic;
using DocoptNet;
using Gemini.Commander.Core.Extensions;

namespace Gemini.Commander.Core
{
    public class Option
    {
        public Option(IDictionary<string, ValueObject> mainArgs)
        {
            Args = mainArgs;
        }

        public IDictionary<string, ValueObject> Args { get; }

        public int Take => Args.Extract("--take", 25);
        public int? Parent => Args.Extract("--parent", -1) == -1 ? null : (int?)Args.Extract("--parent", -1);
        public bool Stemmed => Args.Extract("--stemmed", false);
        public bool IncludeClosed => Args.Extract("--inculed-closed", false);
        public int LogType => Args.Extract("--log-type", 30);
        public int WorkingHours => Args.Extract("--working-hours", 8);
        public DateTime When => Args.Extract("--when", DateTime.Now);
        public DateTime From => DateTime.Today.AddDays(-30);
        public DateTime To => DateTime.Today;

    }
}