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
      magictimes show logs ticket  <id> [my] [--from=<date>] [--to=<date>]
      magictimes show logs user <username> [--from=<date>]  [--to=<date>]
      magictimes show hours my 
      magictimes show hours by <user> [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  
      magictimes show words [--stemmed] [--all]

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
            Arguments = new Argument(args);
        }

        public Option Options { get; set; }
        public Argument Arguments { get; set; }
        public IDictionary<string, ValueObject> Args => args;

        public bool ArgMy => args["my"].IsTrue;
    }

    public static class Ext
    {
        public static T Extract<T>(this IDictionary<string, ValueObject> args, string key, T defaultValue) => args[key]?.Value != null ? (T)Convert.ChangeType(args[key]?.Value?.ToString(), typeof(T)) : defaultValue;
        public static int ConvertTo(this string hours, string tag)
        {
            var pattern = $@"(?<c>\d+){tag}";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Match(hours, pattern, RegexOptions.IgnoreCase).Groups["c"]?.Value;
            return Convert.ToInt32(value ?? "0");
        }
    }

    public class Option
    {
        public Option(IDictionary<string, ValueObject> mainArgs)
        {
            Args = mainArgs;
        }

        public IDictionary<string, ValueObject> Args { get; }

        public int Take => Convert.ToInt32(Args["--take"].ToString());
        public int LogType => Args.Extract("--log-type", 30);
        public int WorkingHours => Args.Extract("--working-hours", 8);
        public DateTime When => Args.Extract("--when", DateTime.Now);
        public DateTime From => DateTime.Today.AddDays(-30);
        public DateTime To => DateTime.Today;
    }

    public class Argument
    {
        public Argument(IDictionary<string, ValueObject> mainArgs)
        {
            Args = mainArgs;
        }

        public IDictionary<string, ValueObject> Args { get; }

        public int Id => Convert.ToInt32(Args["<id>"].ToString());
        public string Message => string.Join(" ", Args["<message>"]?.AsList?.ToArray());
        public int Hours => Args["<time>"].ToString().ConvertTo("h");
        public int Minutes => Args["<time>"].ToString().ConvertTo("m");
    }
}