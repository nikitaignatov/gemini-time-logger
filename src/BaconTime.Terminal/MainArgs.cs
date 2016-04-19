using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocoptNet;

namespace BaconTime.Terminal
{
    public class MainArgs
    {
        public const string Usage = @"Magic Times

    Usage:
      magictimes log <time> <ticket> [--when=<date>] [--log-type=<type>] <message>... 
      magictimes show logged hours <user> [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  

    Options:
      -h --help                 Show this screen.
      --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
      --log-type=<type>         Type of timelogging billable [default:30]
      --from=<date>             The first inclussive date of the time period [default:today-30days]
      --to=<date>           The last inclussive date of the time period [default:today]
      --working-hours=<hours>   The number of working hours in a working day [default:8]
    ";
        private readonly IDictionary<string, ValueObject> args;
        public MainArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false)
        {
            args = new Docopt().Apply(Usage, argv, help, version, optionsFirst, exit);
        }

        public IDictionary<string, ValueObject> Args => args;

        public bool CmdLog => args["log"].IsTrue;
        public bool CmdShowLoggedHours => args["show"].IsTrue && args["logged"].IsTrue && args["hours"].IsTrue;

        public int OptTicket => Convert.ToInt32(args["<ticket>"].ToString());
        public int OptTimeType => Extract("--log-type", 30);
        public DateTime OptWhen => Extract("--when", DateTime.Now);
        public int OptWorkingHOurs => Extract("--working-hours", 8);

        private T Extract<T>(string key, T defaultValue) => args.ContainsKey(key) ? (T)Convert.ChangeType(args[key].Value, typeof(T)) : defaultValue;

        public int OptHours => ConvertTo(args["<time>"].ToString(), "h");
        public int OptMinutes => ConvertTo(args["<time>"].ToString(), "h");

        public DateTime OptFrom => DateTime.Today.AddDays(-30);
        public DateTime OptTo => DateTime.Today;

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