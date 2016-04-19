using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocoptNet;

namespace BaconTime.Terminal
{
    public class MainArgs
    {
        public const string USAGE = @"Magic Times

    Usage:
      magictimes log <time> <ticket> [--when=<date>] [--time-type=<type>] <message>... 

    Options:
      -h --help             Show this screen.
      --when=<date>         The date for wen time log entry YYYY-MM-DD [default:now]
      --time-type=<type>    Type of timelogging billable [default:30]
    ";
        private readonly IDictionary<string, ValueObject> args;
        public MainArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false)
        {
            args = new Docopt().Apply(USAGE, argv, help, version, optionsFirst, exit);
        }

        public IDictionary<string, ValueObject> Args => args;

        public bool CmdLog => args["log"].IsTrue;

        public int OptTicket => Convert.ToInt32(args["<ticket>"].ToString());
        public int OptTimeType => args.ContainsKey("time-type") ? Convert.ToInt32(args["time-type"].ToString()) : 30;
        public int OptHours => ConvertTo(args["<time>"].ToString(), "h");
        public int OptMinutes => ConvertTo(args["<time>"].ToString(), "h");
        public string OptMessage => string.Join(" ", args["<message>"].AsList);
        
        private static int ConvertTo(string hours, string tag)
        {
            var pattern = $@".*(\d+){tag}.*";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Replace(hours, pattern, "$1", RegexOptions.IgnoreCase);
            return Convert.ToInt32(value ?? "0");
        }
    }
}