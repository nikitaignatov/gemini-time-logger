using System;
using System.Collections.Generic;
using DocoptNet;
using Gemini.Commander.Core.Extensions;

namespace Gemini.Commander.Core
{
    public class Argument
    {
        public Argument(IDictionary<string, ValueObject> mainArgs)
        {
            Args = mainArgs;
        }

        public IDictionary<string, ValueObject> Args { get; }

        public int Id => Convert.ToInt32(Args["<id>"].ToString());
        public int Project => Convert.ToInt32(Args["<project>"].ToString());
        public string Message => string.Join(" ", Args["<message>"]?.AsList?.ToArray());
        public string Title => string.Join(" ", Args["<title>"]?.AsList?.ToArray());
        public int Hours => Args["<time>"].ToString().ConvertTo("h");
        public int Minutes => Args["<time>"].ToString().ConvertTo("m");
        public bool My => Args["my"].IsTrue;
    }
}