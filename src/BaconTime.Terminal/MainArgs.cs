namespace BaconTime.Terminal
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using DocoptNet;

    public class MainArgs
    {
        public const string Usage = @"Magic Times

    Usage:
      magictimes log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
      magictimes create ticket <project> <state> <title>...
      magictimes show logs my
      magictimes show logs project <id> [--from=<date>] [--to=<date>]
      magictimes show logs ticket <id> [my] [--from=<date>] [--to=<date>]
      magictimes show logs user <username> [--from=<date>]  [--to=<date>]
      magictimes show hours my 
      magictimes show hours by <user> [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  
      magictimes show words my [--stemmed]
      magictimes show words all [--stemmed]

    Options:
      -h --help                 Show this screen.
      --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
      --log-type=<type>         Type of timelogging billable [default:30]
      --from=<date>             The first inclussive date of the time period [default:today-30days]
      --to=<date>               The last inclussive date of the time period [default:today]
      --working-hours=<hours>   The number of working hours in a working day [default:8]
    ";
        private readonly IDictionary<string, ValueObject> args;
        public MainArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false)
        {
            args = new Docopt().Apply(Usage, argv, help, version, optionsFirst, exit);
            Options = new Option(args);
        }

        public Option Options { get; set; }
        public IDictionary<string, ValueObject> Args => args;
        public class Option
        {
            public Option(IDictionary<string, ValueObject> mainArgs)
            {
                Args = mainArgs;
            }

            public IDictionary<string, ValueObject> Args { get; }

            public int Id => Convert.ToInt32(Args["<id>"].ToString());
            public int Take => Convert.ToInt32(Args["--take"].ToString());
            public int LogType => Extract(Args, "--log-type", 30);
            public int WorkingHours => Extract(Args, "--working-hours", 8);
            public int Hours => ConvertTo(Args["<time>"].ToString(), "h");
            public int Minutes => ConvertTo(Args["<time>"].ToString(), "m");
        }

        public bool ArgMy => args["my"].IsTrue;
        public string OptMessage => string.Join(" ", args["<message>"]?.AsList?.ToArray());

        public DateTime OptWhen => Extract(Args, "--when", DateTime.Now);
        public DateTime OptFrom => DateTime.Today.AddDays(-30);
        public DateTime OptTo => DateTime.Today;

        private static T Extract<T>(IDictionary<string, ValueObject> args, string key, T defaultValue) => args[key]?.Value != null ? (T)Convert.ChangeType(args[key]?.Value?.ToString(), typeof(T)) : defaultValue;

        private static int ConvertTo(string hours, string tag)
        {
            var pattern = $@"(?<c>\d+){tag}";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Match(hours, pattern, RegexOptions.IgnoreCase).Groups["c"]?.Value;
            return Convert.ToInt32(value ?? "0");
        }
    }
}